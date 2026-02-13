document.addEventListener("DOMContentLoaded", function () {

    const calendarEl = document.getElementById("calendarFerie");
    const modalEl = document.getElementById("modaleFerie");
    const modal = new bootstrap.Modal(modalEl);

    let selectedStart = null;
    let selectedEnd = null;
    let selectedId = null;

    // =====================================================
    // FULLCALENDAR
    // =====================================================
    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridMonth",
        locale: "it",
        firstDay: 1, // Inizia dal lunedì
        selectable: true,
        selectMirror: true,
        height: 650,
        headerToolbar: {
            left: "prev,next today",
            center: "title",
            right: ""
        },
        
        // Applica classi CSS personalizzate ai giorni del weekend
        dayCellClassNames: function(arg) {
            const dayOfWeek = arg.date.getDay();
            if (dayOfWeek === 6) { // Sabato
                return ['weekend-day', 'sabato'];
            }
            if (dayOfWeek === 0) { // Domenica
                return ['weekend-day', 'domenica'];
            }
            return [];
        },

        // -------------------------
        // SELEZIONE PER NUOVA RICHIESTA
        // -------------------------
        select: function (info) {
            resetModal();

            selectedStart = info.startStr;
            selectedEnd = info.endStr
                ? new Date(new Date(info.endStr).setDate(new Date(info.endStr).getDate() - 1))
                      .toISOString()
                      .substring(0, 10)
                : info.startStr;

            // Popola i datepicker
            document.getElementById("dataInizioFerie").value = selectedStart;
            document.getElementById("dataFineFerie").value = selectedEnd;

            aggiornaPeriodo();

            modal.show();
        },

        // -------------------------
        // CLICK SU FERIE / PERMESSO
        // -------------------------
        eventClick: function (info) {
            const ev = info.event;

            resetModal();

            selectedId = ev.id;
            selectedStart = ev.start.toISOString().substring(0, 10);
            selectedEnd = ev.end
                ? new Date(ev.end.getTime() - 86400000).toISOString().substring(0, 10)
                : selectedStart;

            document.getElementById("tipoFerie").value = ev.extendedProps.tipo;
            document.getElementById("descrizioneFerie").value = ev.extendedProps.motivo || "";
            
            // Popola i datepicker
            document.getElementById("dataInizioFerie").value = selectedStart;
            document.getElementById("dataFineFerie").value = selectedEnd;
            
            // Mostra/nascondi data fine per permesso
            if (ev.extendedProps.tipo === "Permesso") {
                document.getElementById("dataFineContainer").style.display = "none";
            }

            aggiornaPeriodo();

            document.getElementById("btnRichiediFerie").textContent = "Salva modifiche";
            aggiungiBottoneElimina();

            modal.show();
        },

        // -------------------------
        // EVENTI DA API
        // -------------------------
        eventSources: [
            {
                url: "/Dipendente/CongedoApi/GetRichieste",
                method: "GET"
            }
        ],
        
        // Trasforma i dati prima di renderizzarli
        eventDataTransform: function(eventData) {
            // Modifica il titolo in base allo stato (0=InAttesa, 1=Approvato, 2=Respinto)
            if (eventData.extendedProps) {
                const stato = eventData.extendedProps.stato;
                const tipo = eventData.extendedProps.tipo || eventData.title;
                
                // 2 = Respinto → nascondi l'evento (non mostrare nel calendario)
                if (stato === 2) {
                    return null; // Non renderizzare eventi respinti
                }
                
                // 0 = InAttesa → aggiungi "- Da approvare"
                if (stato === 0) {
                    eventData.title = tipo + ' - In attesa di approvazione';
                } else {
                    eventData.title = tipo;
                }
            }
            
            return eventData;
        },
        
        eventDidMount: function(info) {
            const stato = info.event.extendedProps.stato;
            const tipo = info.event.extendedProps.tipo;
            
            // Nascondi completamente eventi respinti
            if (stato === 2) {
                info.el.style.display = 'none';
                return;
            }
            
            // Applica colori in base allo stato (0=InAttesa, 1=Approvato)
            if (stato === 0) { // InAttesa
                if (tipo === 'Ferie') {
                    info.el.style.backgroundColor = '#dc3545'; // Rosso per ferie in attesa
                    info.el.style.borderColor = '#dc3545';
                } else if (tipo === 'Permesso') {
                    info.el.style.backgroundColor = '#fd7e14'; // Arancione per permesso in attesa
                    info.el.style.borderColor = '#fd7e14';
                }
                info.el.style.color = '#fff';
            } else if (stato === 1) { // Approvato
                if (tipo === 'Ferie') {
                    info.el.style.backgroundColor = '#dc3545'; // Rosso anche per ferie approvate
                    info.el.style.borderColor = '#dc3545';
                } else if (tipo === 'Permesso') {
                    info.el.style.backgroundColor = '#fd7e14'; // Arancione anche per permesso approvato
                    info.el.style.borderColor = '#fd7e14';
                }
                info.el.style.color = '#fff';
            }
        }
    });

    calendar.render();

    // =====================================================
    // GESTIONE CAMBIO TIPO (Ferie/Permesso)
    // =====================================================
    document.getElementById("tipoFerie").addEventListener("change", function() {
        const tipo = this.value;
        const dataFineContainer = document.getElementById("dataFineContainer");
        
        if (tipo === "Permesso") {
            dataFineContainer.style.display = "none";
            // Per permesso, data fine = data inizio
            const dataInizio = document.getElementById("dataInizioFerie").value;
            if (dataInizio) {
                selectedStart = dataInizio;
                selectedEnd = dataInizio;
                document.getElementById("dataFineFerie").value = dataInizio;
                aggiornaPeriodo();
            }
        } else {
            dataFineContainer.style.display = "block";
        }
    });

    // =====================================================
    // GESTIONE DATEPICKER - Data Inizio
    // =====================================================
    document.getElementById("dataInizioFerie").addEventListener("change", function() {
        selectedStart = this.value;
        
        // Se tipo permesso, sincronizza anche data fine
        if (document.getElementById("tipoFerie").value === "Permesso") {
            selectedEnd = selectedStart;
            document.getElementById("dataFineFerie").value = selectedStart;
        } else {
            // Assicurati che data fine non sia prima di data inizio
            const dataFine = document.getElementById("dataFineFerie").value;
            if (dataFine && dataFine < selectedStart) {
                selectedEnd = selectedStart;
                document.getElementById("dataFineFerie").value = selectedStart;
            } else if (dataFine) {
                selectedEnd = dataFine;
            }
        }
        
        aggiornaPeriodo();
    });

    // =====================================================
    // GESTIONE DATEPICKER - Data Fine
    // =====================================================
    document.getElementById("dataFineFerie").addEventListener("change", function() {
        selectedEnd = this.value;
        
        // Assicurati che data fine non sia prima di data inizio
        const dataInizio = document.getElementById("dataInizioFerie").value;
        if (dataInizio && selectedEnd < dataInizio) {
            alert("La data fine non può essere precedente alla data inizio");
            selectedEnd = dataInizio;
            this.value = dataInizio;
        }
        
        aggiornaPeriodo();
    });

    // =====================================================
    // BUTTON: RICHIEDI / SALVA
    // =====================================================
    document.getElementById("btnRichiediFerie").addEventListener("click", async function () {

        const tipo = document.getElementById("tipoFerie").value;
        const motivo = document.getElementById("descrizioneFerie").value;

        const dto = {
            id: selectedId,
            tipo: tipo,
            motivo: motivo,
            dal: selectedStart,
            al: tipo === "Permesso" ? selectedStart : selectedEnd
        };

        const url = selectedId
            ? "/Dipendente/CongedoApi/Update"
            : "/Dipendente/CongedoApi/Richiedi";

        const res = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            alert("Errore nel salvataggio della richiesta");
            return;
        }

        modal.hide();
        calendar.refetchEvents();
    });

    // =====================================================
    // FUNZIONI DI SUPPORTO
    // =====================================================
    function aggiornaPeriodo() {
        document.getElementById("periodoFerie").textContent =
            selectedStart === selectedEnd
                ? selectedStart
                : `${selectedStart} → ${selectedEnd}`;

        const diff =
            (new Date(selectedEnd) - new Date(selectedStart)) / 86400000 + 1;

        document.getElementById("totaleGiorni").textContent = diff;
    }

    function resetModal() {
        selectedId = null;
        selectedStart = null;
        selectedEnd = null;

        document.getElementById("tipoFerie").value = "Ferie";
        document.getElementById("descrizioneFerie").value = "";
        document.getElementById("dataInizioFerie").value = "";
        document.getElementById("dataFineFerie").value = "";
        document.getElementById("dataFineContainer").style.display = "block";
        document.getElementById("btnRichiediFerie").textContent = "Richiedi";

        const btnDel = document.getElementById("btnEliminaFerie");
        if (btnDel) btnDel.remove();
    }

    function aggiungiBottoneElimina() {
        if (document.getElementById("btnEliminaFerie")) return;

        const btn = document.createElement("button");
        btn.id = "btnEliminaFerie";
        btn.className = "btn btn-danger me-auto";
        btn.textContent = "Elimina";

        btn.onclick = async function () {
            if (!confirm("Vuoi eliminare questa richiesta?")) return;

            const res = await fetch("/Dipendente/CongedoApi/Delete", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(selectedId)
            });

            if (!res.ok) {
                alert("Errore eliminazione");
                return;
            }

            modal.hide();
            calendar.refetchEvents();
        };

        document.querySelector(".modal-footer").prepend(btn);
    }
});
