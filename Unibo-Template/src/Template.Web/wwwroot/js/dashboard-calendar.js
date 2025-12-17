// =============================================================
//  FullCalendar + Popup attività + API reali
// =============================================================

let calendar;

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
    document.getElementById("oraInizio").value = "";
    document.getElementById("oraFine").value = "";
    document.getElementById("progetto").value = "";
    document.getElementById("cliente").value = "";
    document.getElementById("attivita").value = "";
    document.getElementById("descrizione").value = "";

    document.getElementById("checkTrasferta").checked = false;
    document.getElementById("boxTrasferta").style.display = "none";

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

    let dto = {
        id: id ? parseInt(id) : 0,
        giorno: giorno,
        oraInizio: document.getElementById("oraInizio").value,
        oraFine: document.getElementById("oraFine").value,
        progetto: document.getElementById("progetto").value,
        cliente: document.getElementById("cliente").value,
        attivita: document.getElementById("attivita").value,
        descrizione: document.getElementById("descrizione").value,
        trasferta: document.getElementById("checkTrasferta").checked,
        spesaTrasporto: document.getElementById("spesaTrasporto").value || 0,
        spesaCibo: document.getElementById("spesaVitto").value || 0,
        spesaAlloggio: document.getElementById("spesaAlloggio").value || 0
    };

    const url = id
        ? "/Dipendente/CalendarioApi/UpdateAttivita"
        : "/Dipendente/CalendarioApi/AddAttivita";

    const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto)
    });

    if (res.ok) {
        const modal = bootstrap.Modal.getInstance(document.getElementById("modaleOre"));
        modal.hide();

        calendar.removeAllEvents();

        fetch("/Dipendente/CalendarioApi/GetAttivita")
            .then(r => r.json())
            .then(lista => lista.forEach(ev => calendar.addEvent(ev)));

    } else {
        alert("Errore nel salvataggio.");
    }
});

// ----------------------
//  ELIMINA ATTIVITÀ
// ----------------------
document.getElementById("btnElimina").addEventListener("click", async function () {

    const id = this.getAttribute("data-id");
    if (!id) return;

    if (!confirm("Vuoi davvero eliminare questa attività?")) return;

    const res = await fetch("/Dipendente/CalendarioApi/DeleteAttivita", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(parseInt(id))
    });

    if (res.ok) {
        location.reload();
    }
});

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

        dateClick: info => apriPopupPerData(info.dateStr),

        eventClick: function (info) {

            const ev = info.event;

            const start = ev.start;
            const end = ev.end ?? new Date(start.getTime() + 60 * 60 * 1000);

            // ORA CORRETTA (NO UTC!)
            document.getElementById("oraInizio").value = formatTimeLocal(start);
            document.getElementById("oraFine").value = formatTimeLocal(end);

            document.getElementById("progetto").value = ev.extendedProps.progetto ?? "";
            document.getElementById("cliente").value = ev.extendedProps.cliente ?? "";
            document.getElementById("attivita").value = ev.extendedProps.attivita ?? "";
            document.getElementById("descrizione").value = ev.extendedProps.descrizione ?? "";

            document.getElementById("checkTrasferta").checked = ev.extendedProps.trasferta ?? false;
            document.getElementById("boxTrasferta").style.display =
                ev.extendedProps.trasferta ? "block" : "none";

            document.getElementById("btnSalva").setAttribute("data-id", ev.id);
            document.getElementById("btnSalva").setAttribute("data-date", start.toISOString().substring(0, 10));

            const btnElimina = document.getElementById("btnElimina");
            btnElimina.style.display = "inline-block";
            btnElimina.setAttribute("data-id", ev.id);

            new bootstrap.Modal(document.getElementById("modaleOre")).show();
        }
    });

    calendar.render();
    aggiornaTitoloCalendario();

    // Caricamento eventi
    fetch("/Dipendente/CalendarioApi/GetAttivita")
        .then(r => r.json())
        .then(lista => lista.forEach(ev => calendar.addEvent(ev)));

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
