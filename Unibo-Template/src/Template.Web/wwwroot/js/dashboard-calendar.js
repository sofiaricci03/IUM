// =============================================================
//  FullCalendar + Popup attività + API reali - VERSIONE DINAMICA
// =============================================================

let calendar;

// Detect current area (Dipendente or Responsabile)
const currentArea = window.location.pathname.includes('/Responsabile/') ? 'Responsabile' : 'Dipendente';

// ----------------------
//  FUNZIONE: Arrotonda a 15 minuti
// ----------------------
function roundToQuarter(time) {
    if (!time) return '';
    const [hours, minutes] = time.split(':');
    const roundedMinutes = Math.round(parseInt(minutes) / 15) * 15;
    const finalMinutes = roundedMinutes === 60 ? 0 : roundedMinutes;
    const finalHours = roundedMinutes === 60 ? (parseInt(hours) + 1) % 24 : parseInt(hours);
    return `${String(finalHours).padStart(2, '0')}:${String(finalMinutes).padStart(2, '0')}`;
}

// ----------------------
//  FUNZIONE: Calcola durata in ore
// ----------------------
function calcolaDurata(start, end) {
    if (!start || !end) return 0;
    const [h1, m1] = start.split(':').map(Number);
    const [h2, m2] = end.split(':').map(Number);
    const minuti = (h2 * 60 + m2) - (h1 * 60 + m1);
    return (minuti / 60).toFixed(2);
}

// ----------------------
//  FUNZIONE: Colore per progetto
// ----------------------
function getColorForProject(projectName) {
    const colors = [
        '#3788d8', '#28a745', '#fd7e14', '#6f42c1', 
        '#e83e8c', '#20c997', '#17a2b8', '#ffc107'
    ];
    let hash = 0;
    for (let i = 0; i < projectName.length; i++) {
        hash = projectName.charCodeAt(i) + ((hash << 5) - hash);
    }
    return colors[Math.abs(hash) % colors.length];
}

// ----------------------
//  FUNZIONE UTILE: format orari senza UTC
// ----------------------
function formatTimeLocal(date) {
    return date.toLocaleTimeString("it-IT", { hour: "2-digit", minute: "2-digit", hour12: false });
}

// ----------------------
//  AGGIORNA TITOLO MESE
// ----------------------
function aggiornaTitoloCalendario() {
    const titolo = document.getElementById("titleCalendar");
    const data = calendar.getDate();

    const nomeMese = data.toLocaleDateString("it-IT", {
        month: "long",
        year: "numeric"
    });

    titolo.textContent = nomeMese.charAt(0).toUpperCase() + nomeMese.slice(1);
}

// ----------------------
//  APRI POPUP PER NUOVA ATTIVITÀ
// ----------------------
function apriPopupPerData(dateStr) {
    document.getElementById("oraInizio").value = "09:00";
    document.getElementById("oraFine").value = "18:00";
    document.getElementById("progetto").value = "";
    document.getElementById("cliente").value = "";
    document.getElementById("attivita").value = "";
    document.getElementById("descrizione").value = "";

    document.getElementById("checkTrasferta").checked = false;
    document.getElementById("boxTrasferta").style.display = "none";
    document.getElementById("spesaTrasporto").value = "";
    document.getElementById("spesaVitto").value = "";
    document.getElementById("spesaAlloggio").value = "";

    document.getElementById("btnSalva").removeAttribute("data-id");
    document.getElementById("btnSalva").setAttribute("data-date", dateStr);

    const lbl = document.getElementById("dataSelezionata");
    if (lbl) lbl.textContent = new Date(dateStr).toLocaleDateString("it-IT");

    document.getElementById("btnElimina").style.display = "none";

    new bootstrap.Modal(document.getElementById("modaleOre")).show();
}

// ----------------------
//  SALVA ATTIVITÀ (CREATE/UPDATE)
// ----------------------
document.getElementById("btnSalva").addEventListener("click", async function () {

    const id = this.getAttribute("data-id");
    const giorno = this.getAttribute("data-date");
    
    // Ottieni progettoId dal select
    const progettoSelect = document.getElementById("progetto");
    const progettoId = progettoSelect.value;
    const progettoNome = progettoSelect.options[progettoSelect.selectedIndex]?.text || "";

    let dto = {
        id: id ? parseInt(id) : 0,
        giorno: giorno,
        oraInizio: document.getElementById("oraInizio").value,
        oraFine: document.getElementById("oraFine").value,
        progettoId: progettoId ? parseInt(progettoId) : 0,
        progetto: progettoNome,
        cliente: document.getElementById("cliente").value,
        attivita: document.getElementById("attivita").value,
        descrizione: document.getElementById("descrizione").value || "",
        trasferta: document.getElementById("checkTrasferta").checked,
        spesaTrasporto: parseFloat(document.getElementById("spesaTrasporto").value) || 0,
        spesaCibo: parseFloat(document.getElementById("spesaVitto").value) || 0,
        spesaAlloggio: parseFloat(document.getElementById("spesaAlloggio").value) || 0
    };

    // Validazione
    if (!dto.progettoId || !dto.cliente || !dto.attivita) {
        alert("Compila tutti i campi obbligatori!");
        return;
    }

    const durata = calcolaDurata(dto.oraInizio, dto.oraFine);
    if (durata <= 0) {
        alert("L'orario di fine deve essere dopo l'orario di inizio!");
        return;
    }

    const url = id
        ? `/${currentArea}/CalendarioApi/UpdateAttivita`
        : `/${currentArea}/CalendarioApi/AddAttivita`;

    try {
        const res = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const data = await res.json();

        if (res.ok) {
            const modal = bootstrap.Modal.getInstance(document.getElementById("modaleOre"));
            modal.hide();

            // Ricarica calendario
            calendar.removeAllEvents();
            caricaEventi();
            
            alert(data.message || "Attività salvata!");
        } else {
            alert(data.error || "Errore nel salvataggio");
        }
    } catch (error) {
        console.error("Errore:", error);
        alert("Errore di connessione");
    }
});

// ----------------------
//  ELIMINA ATTIVITÀ
// ----------------------
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
        const modal = bootstrap.Modal.getInstance(document.getElementById("modaleOre"));
        modal.hide();
        
        calendar.removeAllEvents();
        caricaEventi();
    }
});

// ----------------------
//  CARICA EVENTI NEL CALENDARIO
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
//  INIZIALIZZAZIONE CALENDARIO
// ----------------------
document.addEventListener("DOMContentLoaded", function () {

    const calendarEl = document.getElementById("calendar");

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridMonth",
        locale: "it",
        selectable: true,
        height: 650,
        headerToolbar: false,
        
        eventTimeFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        },

        dateClick: info => apriPopupPerData(info.dateStr),

        eventClick: function (info) {

            const ev = info.event;

            const start = ev.start;
            const end = ev.end ?? new Date(start.getTime() + 60 * 60 * 1000);

            // Popola orari
            document.getElementById("oraInizio").value = formatTimeLocal(start);
            document.getElementById("oraFine").value = formatTimeLocal(end);

            // Popola progetto (usa ID, non nome)
            const progettoId = ev.extendedProps.progettoId || 0;
            document.getElementById("progetto").value = progettoId;
            
            // Trigger change per auto-compilare cliente
            const event = new Event('change');
            document.getElementById("progetto").dispatchEvent(event);

            // Popola altri campi
            document.getElementById("attivita").value = ev.extendedProps.attivita || "";
            document.getElementById("descrizione").value = ev.extendedProps.descrizione || "";

            // Trasferta
            const trasferta = ev.extendedProps.trasferta || false;
            document.getElementById("checkTrasferta").checked = trasferta;
            document.getElementById("boxTrasferta").style.display = trasferta ? "block" : "none";
            
            if (trasferta) {
                document.getElementById("spesaTrasporto").value = ev.extendedProps.spesaTrasporto || 0;
                document.getElementById("spesaVitto").value = ev.extendedProps.spesaCibo || 0;
                document.getElementById("spesaAlloggio").value = ev.extendedProps.spesaAlloggio || 0;
            }

            // Imposta ID per modifica
            document.getElementById("btnSalva").setAttribute("data-id", ev.id);
            document.getElementById("btnSalva").setAttribute("data-date", start.toISOString().substring(0, 10));

            const btnElimina = document.getElementById("btnElimina");
            btnElimina.style.display = "inline-block";
            btnElimina.setAttribute("data-id", ev.id);

            const lbl = document.getElementById("dataSelezionata");
            if (lbl) lbl.textContent = start.toLocaleDateString("it-IT");

            new bootstrap.Modal(document.getElementById("modaleOre")).show();
        },
        
        // Totale ore per giorno
        dayCellDidMount: function(info) {
            const dateStr = info.date.toISOString().split('T')[0];
            const events = calendar.getEvents().filter(e => 
                e.start && e.start.toISOString().split('T')[0] === dateStr
            );
            
            if (events.length > 0) {
                let totalMinutes = 0;
                events.forEach(e => {
                    if (e.start && e.end) {
                        totalMinutes += (e.end - e.start) / (1000 * 60);
                    }
                });
                const hours = (totalMinutes / 60).toFixed(1);
                
                const badge = document.createElement('div');
                badge.className = 'day-total-hours';
                badge.textContent = `${hours}h`;
                badge.style.cssText = `
                    position: absolute;
                    bottom: 2px;
                    right: 2px;
                    background: ${hours >= 6 ? '#28a745' : '#ffc107'};
                    color: white;
                    padding: 2px 6px;
                    border-radius: 10px;
                    font-size: 0.7rem;
                    font-weight: 600;
                `;
                info.el.style.position = 'relative';
                info.el.appendChild(badge);
            }
        }
    });

    calendar.render();
    aggiornaTitoloCalendario();

    // Carica eventi iniziali
    caricaEventi();

    // Navigazione
    document.getElementById("prevBtn").addEventListener("click", () => {
        calendar.prev();
        aggiornaTitoloCalendario();
    });

    document.getElementById("nextBtn").addEventListener("click", () => {
        calendar.next();
        aggiornaTitoloCalendario();
    });

    // Cambio vista
    document.getElementById("btnGiorno").addEventListener("click", () => calendar.changeView("timeGridDay"));
    document.getElementById("btnSettimana").addEventListener("click", () => calendar.changeView("timeGridWeek"));
    document.getElementById("btnMese").addEventListener("click", () => calendar.changeView("dayGridMonth"));
});