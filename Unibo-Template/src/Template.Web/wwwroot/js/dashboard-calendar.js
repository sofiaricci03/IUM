// =============================================================
//  FullCalendar + Pallini Rendicontazione - VERSIONE COMPLETA
// =============================================================

let calendar;
let statoGiorni = {};
let currentYear = new Date().getFullYear();
let currentMonth = new Date().getMonth() + 1;

const currentArea = window.CURRENT_AREA || 'Dipendente';

// ----------------------
//  UTILITY FUNCTIONS
// ----------------------
function calcolaDurata(start, end) {
    if (!start || !end) return 0;
    const [h1, m1] = start.split(':').map(Number);
    const [h2, m2] = end.split(':').map(Number);
    const minuti = (h2 * 60 + m2) - (h1 * 60 + m1);
    return (minuti / 60).toFixed(2);
}

function getColorForProject(projectName) {
    const colors = ['#3788d8', '#28a745', '#fd7e14', '#6f42c1', '#e83e8c', '#20c997', '#17a2b8', '#ffc107'];
    let hash = 0;
    for (let i = 0; i < projectName.length; i++) {
        hash = projectName.charCodeAt(i) + ((hash << 5) - hash);
    }
    return colors[Math.abs(hash) % colors.length];
}

function formatTimeLocal(date) {
    return date.toLocaleTimeString("it-IT", { hour: "2-digit", minute: "2-digit", hour12: false });
}

function aggiornaTitoloCalendario() {
    const titolo = document.getElementById("titleCalendar");
    if (!titolo) return;
    const data = calendar.getDate();
    const nomeMese = data.toLocaleDateString("it-IT", { month: "long", year: "numeric" });
    titolo.textContent = nomeMese.charAt(0).toUpperCase() + nomeMese.slice(1);
}

// ----------------------
//  RENDICONTAZIONE
// ----------------------
async function caricaStatoGiorni() {
    try {
        const response = await fetch(`/${currentArea}/CalendarioApi/GetStatoGiorni?anno=${currentYear}&mese=${currentMonth}`);
        const data = await response.json();
        
        statoGiorni = data.statoGiorni || {};
        
        const alertRendiconta = document.getElementById('alertRendiconta');
        if (alertRendiconta) {
            alertRendiconta.classList.toggle('d-none', !data.mostraRendiconta);
        }

        const alertStato = document.getElementById('alertStatoRendicontazione');
        if (alertStato) {
            if (data.statoRendicontazione === 'Inviata') {
                alertStato.className = 'alert alert-info mb-3';
                alertStato.innerHTML = `<i class="fa-solid fa-clock"></i> <strong>Rendicontazione inviata</strong> - In attesa di approvazione${data.dataInvio ? ' il ' + data.dataInvio : ''}`;
            } else if (data.statoRendicontazione === 'Approvata') {
                alertStato.className = 'alert alert-success mb-3';
                alertStato.innerHTML = `<i class="fa-solid fa-circle-check"></i> <strong>Rendicontazione approvata</strong>${data.dataInvio ? ' il ' + data.dataInvio : ''}`;
            } else if (data.statoRendicontazione === 'Respinta') {
                alertStato.className = 'alert alert-danger mb-3';
                let msg = '<i class="fa-solid fa-circle-xmark"></i> <strong>Rendicontazione respinta</strong>';
                if (data.noteResponsabile) msg += `<br><small>Motivazione: ${data.noteResponsabile}</small>`;
                alertStato.innerHTML = msg;
            } else {
                alertStato.classList.add('d-none');
            }
        }

        setTimeout(() => aggiungiPalliniCalendario(), 100);
    } catch (error) {
        console.error('Errore caricamento stato giorni:', error);
    }
}

function aggiungiPalliniCalendario() {
    document.querySelectorAll('.status-indicator').forEach(el => el.remove());
    
    Object.keys(statoGiorni).forEach(dataStr => {
        const stato = statoGiorni[dataStr].stato;
        const cell = document.querySelector(`[data-date="${dataStr}"]`);
        if (!cell) return;
        
        const indicator = document.createElement('div');
        indicator.className = `status-indicator status-${stato}`;
        const tooltips = {
            'completo': 'Giornata completa (6+ ore)',
            'parziale': 'Giornata parziale (<6 ore)',
            'mancante': 'Nessuna attività inserita',
            'congedo': 'Giorno di congedo'
        };
        indicator.title = tooltips[stato] || '';
        
        const dayFrame = cell.querySelector('.fc-daygrid-day-frame');
        if (dayFrame) dayFrame.appendChild(indicator);
    });
}

async function inviaRendicontazione() {
    if (!confirm('Sei sicuro di voler inviare la rendicontazione?\n\nDopo l\'invio non potrai più modificare le attività del mese fino all\'approvazione.')) return;

    try {
        const response = await fetch('/Dipendente/Rendicontazione/InviaRendicontazione', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `anno=${currentYear}&mese=${currentMonth}`
        });
        const data = await response.json();
        
        if (response.ok) {
            alert('✓ ' + data.message);
            await caricaStatoGiorni();
            calendar.refetchEvents();
        } else {
            alert('✗ ' + (data.error || 'Errore durante l\'invio'));
        }
    } catch (error) {
        console.error('Errore invio rendicontazione:', error);
        alert('✗ Errore durante l\'invio della rendicontazione');
    }
}

function onAttivitaSalvataConSuccesso() {
    calendar.removeAllEvents();
    caricaEventi();
    caricaStatoGiorni();
}

// ----------------------
//  APRI POPUP PER NUOVA ATTIVITÀ
// ----------------------
function apriPopupPerData(dateStr) {
    const oraInizio = document.getElementById("oraInizio");
    const oraFine = document.getElementById("oraFine");
    const progetto = document.getElementById("progetto");
    const descrizione = document.getElementById("descrizione");
    const checkTrasferta = document.getElementById("checkTrasferta");
    const boxTrasferta = document.getElementById("boxTrasferta");
    const spesaTrasporto = document.getElementById("spesaTrasporto");
    const spesaVitto = document.getElementById("spesaVitto");
    const spesaAlloggio = document.getElementById("spesaAlloggio");
    const btnSalva = document.getElementById("btnSalva");
    const btnElimina = document.getElementById("btnElimina");
    const dataSelezionata = document.getElementById("dataSelezionata");
    
    if (!oraInizio || !oraFine || !progetto) {
        console.error("Elementi del modale non trovati!");
        return;
    }
    
    oraInizio.value = "09:00";
    oraFine.value = "18:00";
    progetto.value = "";
    if (descrizione) descrizione.value = "";
    if (checkTrasferta) checkTrasferta.checked = false;
    if (boxTrasferta) boxTrasferta.style.display = "none";
    if (spesaTrasporto) spesaTrasporto.value = "";
    if (spesaVitto) spesaVitto.value = "";
    if (spesaAlloggio) spesaAlloggio.value = "";
    
    if (btnSalva) {
        btnSalva.removeAttribute("data-id");
        btnSalva.setAttribute("data-date-start", dateStr);
        btnSalva.setAttribute("data-date-end", dateStr);
    }
    
    if (dataSelezionata) {
        dataSelezionata.textContent = new Date(dateStr + 'T00:00:00').toLocaleDateString("it-IT");
    }
    
    if (btnElimina) btnElimina.style.display = "none";
    
    new bootstrap.Modal(document.getElementById("modaleOre")).show();
}

// ----------------------
//  APRI POPUP PER RANGE DATE
// ----------------------
function apriPopupPerRange(startStr, endStr) {
    const start = new Date(startStr + 'T00:00:00');
    let end = new Date(endStr + 'T00:00:00');

    if (end < start) end = new Date(start);

    const giorni = [];
    for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
        giorni.push(new Date(d).toISOString().split('T')[0]);
    }

    const oraInizio = document.getElementById("oraInizio");
    const oraFine = document.getElementById("oraFine");
    const progetto = document.getElementById("progetto");
    const descrizione = document.getElementById("descrizione");
    const checkTrasferta = document.getElementById("checkTrasferta");
    const boxTrasferta = document.getElementById("boxTrasferta");
    const spesaTrasporto = document.getElementById("spesaTrasporto");
    const spesaVitto = document.getElementById("spesaVitto");
    const spesaAlloggio = document.getElementById("spesaAlloggio");
    const btnSalva = document.getElementById("btnSalva");
    const btnElimina = document.getElementById("btnElimina");
    const dataSelezionata = document.getElementById("dataSelezionata");

    if (!oraInizio || !oraFine || !progetto) {
        console.error("Elementi del modale non trovati!");
        return;
    }

    oraInizio.value = "09:00";
    oraFine.value = "18:00";
    progetto.value = "";
    if (descrizione) descrizione.value = "";
    if (checkTrasferta) checkTrasferta.checked = false;
    if (boxTrasferta) boxTrasferta.style.display = "none";
    if (spesaTrasporto) spesaTrasporto.value = "";
    if (spesaVitto) spesaVitto.value = "";
    if (spesaAlloggio) spesaAlloggio.value = "";

    if (btnSalva) {
        btnSalva.removeAttribute("data-id");
        btnSalva.setAttribute("data-date-start", startStr);
        btnSalva.setAttribute("data-date-end", end.toISOString().split('T')[0]);
    }

    if (dataSelezionata) {
        dataSelezionata.textContent = giorni.length === 1 ? start.toLocaleDateString("it-IT") : 
            `${start.toLocaleDateString("it-IT")} → ${end.toLocaleDateString("it-IT")} (${giorni.length} giorni)`;
    }

    if (btnElimina) btnElimina.style.display = "none";

    new bootstrap.Modal(document.getElementById("modaleOre")).show();
}

// ----------------------
//  SALVA/ELIMINA
// ----------------------
document.getElementById("btnSalva").addEventListener("click", async function () {
    const id = this.getAttribute("data-id");
    const giornoSingolo = this.getAttribute("data-date");
    const dateStart = this.getAttribute("data-date-start");
    const dateEnd = this.getAttribute("data-date-end");
    
    const progettoSelect = document.getElementById("progetto");
    const progettoId = progettoSelect.value;
    const selectedOption = progettoSelect.options[progettoSelect.selectedIndex];
const progettoNome = selectedOption?.getAttribute('data-nome') || "";
const clienteNome = selectedOption?.getAttribute('data-cliente') || "";

let dto = {
    id: id ? parseInt(id) : 0,
    giorno: giornoSingolo || dateStart,
    oraInizio: document.getElementById("oraInizio").value,
    oraFine: document.getElementById("oraFine").value,
    progettoId: progettoId ? parseInt(progettoId) : 0,
    progetto: progettoNome,
    cliente: clienteNome,
    attivita: "Attività progetto", // ← FISSO
    descrizione: document.getElementById("descrizione").value || "",
    trasferta: document.getElementById("checkTrasferta").checked,
    spesaTrasporto: parseFloat(document.getElementById("spesaTrasporto").value) || 0,
    spesaCibo: parseFloat(document.getElementById("spesaVitto").value) || 0,
    spesaAlloggio: parseFloat(document.getElementById("spesaAlloggio").value) || 0
};

    if (!dto.progettoId) {
        alert("Seleziona un progetto!");
        return;
    }

    const durata = calcolaDurata(dto.oraInizio, dto.oraFine);
    if (durata <= 0) {
        alert("L'orario di fine deve essere dopo l'orario di inizio!");
        return;
    }

    // Range di date
    if (!id && dateStart && dateEnd) {
        const start = new Date(dateStart);
        const end = new Date(dateEnd);
        const giorni = [];
        
        for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
            giorni.push(new Date(d).toISOString().split('T')[0]);
        }
        
        let successCount = 0;
        let errorMessages = [];
        
        for (const giorno of giorni) {
            dto.giorno = giorno;
                console.log("DTO singolo:", dto);
            
            try {
                const res = await fetch(`/${currentArea}/CalendarioApi/AddAttivita`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(dto)
                });
                
                const data = await res.json();
                if (res.ok) successCount++;
                else errorMessages.push(`${giorno}: ${data.error}`);
            } catch (error) {
                errorMessages.push(`${giorno}: Errore di connessione`);
            }
        }
        
        bootstrap.Modal.getInstance(document.getElementById("modaleOre")).hide();
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
        document.body.classList.remove('modal-open');
        document.body.style.overflow = '';
        
        onAttivitaSalvataConSuccesso();
        
        if (errorMessages.length > 0) {
            alert(`Salvate ${successCount}/${giorni.length} attività.\n\nErrori:\n${errorMessages.join('\n')}`);
        } else {
            alert(`${successCount} attività salvate con successo!`);
        }
        return;
    }

    // Salvataggio singolo
    const url = id ? `/${currentArea}/CalendarioApi/UpdateAttivita` : `/${currentArea}/CalendarioApi/AddAttivita`;

    try {
        const res = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const data = await res.json();

        if (res.ok) {
            bootstrap.Modal.getInstance(document.getElementById("modaleOre")).hide();
            document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
            document.body.classList.remove('modal-open');
            document.body.style.overflow = '';
            onAttivitaSalvataConSuccesso();
            alert(data.message || "Attività salvata!");
        } else {
            alert(data.error || "Errore nel salvataggio");
        }
    } catch (error) {
        console.error("Errore:", error);
        alert("Errore di connessione");
    }
});

document.getElementById("btnElimina").addEventListener("click", async function () {
    const id = this.getAttribute("data-id");
    if (!id) return;
    if (!confirm("Vuoi davvero eliminare questa attività?")) return;

    const res = await fetch(`/${currentArea}/CalendarioApi/DeleteAttivita`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(parseInt(id))
    });

    if (res.ok) {
        bootstrap.Modal.getInstance(document.getElementById("modaleOre")).hide();
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
        document.body.classList.remove('modal-open');
        document.body.style.overflow = '';
        onAttivitaSalvataConSuccesso();
    }
});

// ----------------------
//  CARICA EVENTI
// ----------------------
function caricaEventi() {
    fetch(`/${currentArea}/CalendarioApi/GetAttivita`)
        .then(r => r.json())
        .then(lista => {
            lista.forEach(ev => {
                calendar.addEvent({
                    id: ev.id,
                    title: `${ev.progetto} - ${ev.attivita}`,
                    start: ev.start,
                    end: ev.end,
                    backgroundColor: getColorForProject(ev.progetto),
                    borderColor: getColorForProject(ev.progetto),
                    extendedProps: {
                        progettoId: ev.progettoId || 0,
                        progetto: ev.progetto,
                        cliente: ev.cliente,
                        attivita: ev.attivita,
                        descrizione: ev.descrizione,
                        trasferta: ev.trasferta,
                        spesaTrasporto: ev.spesaTrasporto,
                        spesaCibo: ev.spesaCibo,
                        spesaAlloggio: ev.spesaAlloggio
                    }
                });
            });
        });
}

// ----------------------
//  INIZIALIZZAZIONE
// ----------------------
document.addEventListener("DOMContentLoaded", function () {
    const calendarEl = document.getElementById("calendar");

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridMonth",
        locale: "it",
        firstDay: 1,
        selectable: true,
        selectMirror: true,
        selectOverlap: false,
        height: 650,
        headerToolbar: false,
        
        dayCellClassNames: function(arg) {  
            const dayOfWeek = arg.date.getDay();
            if (dayOfWeek === 6) return ['weekend-day', 'sabato'];
            if (dayOfWeek === 0) return ['weekend-day', 'domenica'];
            return [];
        },
        
        eventTimeFormat: { hour: '2-digit', minute: '2-digit', hour12: false },
        
        select: function(info) {
            apriPopupPerRange(info.startStr, info.endStr);
            calendar.unselect();
        },
        
        dateClick: info => apriPopupPerData(info.dateStr),
        
        eventClick: function (info) {
            const ev = info.event;
            const start = ev.start;
            const end = ev.end ?? new Date(start.getTime() + 60 * 60 * 1000);

            document.getElementById("oraInizio").value = formatTimeLocal(start);
            document.getElementById("oraFine").value = formatTimeLocal(end);
            document.getElementById("progetto").value = ev.extendedProps.progettoId || 0;
            document.getElementById("descrizione").value = ev.extendedProps.descrizione || "";

            const trasferta = ev.extendedProps.trasferta || false;
            document.getElementById("checkTrasferta").checked = trasferta;
            document.getElementById("boxTrasferta").style.display = trasferta ? "block" : "none";
            
            if (trasferta) {
                document.getElementById("spesaTrasporto").value = ev.extendedProps.spesaTrasporto || 0;
                document.getElementById("spesaVitto").value = ev.extendedProps.spesaCibo || 0;
                document.getElementById("spesaAlloggio").value = ev.extendedProps.spesaAlloggio || 0;
            }

            document.getElementById("btnSalva").setAttribute("data-id", ev.id);
            document.getElementById("btnSalva").setAttribute("data-date", start.toISOString().substring(0, 10));
            document.getElementById("btnElimina").style.display = "inline-block";
            document.getElementById("btnElimina").setAttribute("data-id", ev.id);
            document.getElementById("dataSelezionata").textContent = start.toLocaleDateString("it-IT");

            new bootstrap.Modal(document.getElementById("modaleOre")).show();
        },
        
        datesSet: function(info) {
            currentYear = info.view.currentStart.getFullYear();
            currentMonth = info.view.currentStart.getMonth() + 1;
            caricaStatoGiorni();
        },
        
        viewDidMount: function() {
            setTimeout(() => aggiungiPalliniCalendario(), 100);
        },
        
        dayCellDidMount: function(info) {
            const dateStr = info.date.toISOString().split('T')[0];
            const events = calendar.getEvents().filter(e => 
                e.start && e.start.toISOString().split('T')[0] === dateStr
            );
            
            if (events.length > 0) {
                let totalMinutes = 0;
                events.forEach(e => {
                    if (e.start && e.end) totalMinutes += (e.end - e.start) / (1000 * 60);
                });
                const hours = (totalMinutes / 60).toFixed(1);
                
                const badge = document.createElement('div');
                badge.className = 'day-total-hours';
                badge.textContent = `${hours}h`;
                badge.style.cssText = `position:absolute;bottom:2px;right:2px;background:${hours >= 6 ? '#28a745' : '#ffc107'};color:white;padding:2px 6px;border-radius:10px;font-size:0.7rem;font-weight:600;z-index:3;`;
                info.el.style.position = 'relative';
                info.el.appendChild(badge);
            }
        },
        
        eventDidMount: function() {
            setTimeout(() => aggiungiPalliniCalendario(), 50);
        }
    });

    calendar.render();
    aggiornaTitoloCalendario();
    caricaEventi();
    
    // Imposta il pulsante della vista iniziale (Mese) come attivo
    document.getElementById("btnMese").classList.remove("btn-outline-primary");
    document.getElementById("btnMese").classList.add("btn-primary");
    
    const btnRendiconta = document.getElementById('btnRendiconta');
    if (btnRendiconta) btnRendiconta.addEventListener('click', inviaRendicontazione);
    
    caricaStatoGiorni();
    // ========================================
    // FIX: Rimuovi backdrop quando modale si chiude
    // ========================================
    const modaleOre = document.getElementById('modaleOre');
    if (modaleOre) {
        modaleOre.addEventListener('hidden.bs.modal', function() {
            document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
            document.body.classList.remove('modal-open');
            document.body.style.overflow = '';
            document.body.style.paddingRight = '';
        });
    }

    document.getElementById("prevBtn").addEventListener("click", () => { 
        calendar.prev(); 
        aggiornaTitoloCalendario(); 
    });
    
    document.getElementById("nextBtn").addEventListener("click", () => { calendar.next(); aggiornaTitoloCalendario(); });
    
    document.getElementById("btnSettimana").addEventListener("click", () => {
        calendar.changeView("timeGridWeek");
        document.getElementById("btnSettimana").classList.remove("btn-outline-primary");
        document.getElementById("btnSettimana").classList.add("btn-primary");
        document.getElementById("btnMese").classList.remove("btn-primary");
        document.getElementById("btnMese").classList.add("btn-outline-primary");
    });
    
    document.getElementById("btnMese").addEventListener("click", () => {
        calendar.changeView("dayGridMonth");
        document.getElementById("btnMese").classList.remove("btn-outline-primary");
        document.getElementById("btnMese").classList.add("btn-primary");
        document.getElementById("btnSettimana").classList.remove("btn-primary");
        document.getElementById("btnSettimana").classList.add("btn-outline-primary");
    });
    
    document.getElementById("checkTrasferta").addEventListener("change", function() {
        document.getElementById("boxTrasferta").style.display = this.checked ? "block" : "none";
    });
});