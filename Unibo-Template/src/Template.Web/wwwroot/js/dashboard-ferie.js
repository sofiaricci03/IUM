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
        selectable: true,
        height: 650,
        headerToolbar: {
            left: "prev,next today",
            center: "title",
            right: ""
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
        ]
    });

    calendar.render();

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
