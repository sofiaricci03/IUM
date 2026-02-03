/**
 * fatturazione.js
 * Gestione completa della pagina Fatturazione Responsabile
 */

let progettoSelezionato = null;
let previewData = null;

// ===== INIZIALIZZAZIONE =====
$(document).ready(function() {
    loadProgetti();
});

// ===== CARICAMENTO PROGETTI =====
async function loadProgetti() {
    try {
        // Endpoint per ottenere lista progetti con ore rendicontate
        const response = await fetch('/Responsabile/Fatturazione/GetProgetti');
        const progetti = await response.json();

        renderProgetti(progetti);
    } catch (error) {
        console.error('Errore caricamento progetti:', error);
        showError('Errore nel caricamento dei progetti');
    }
}

function renderProgetti(progetti) {
    const container = $('#progetti-container');
    container.empty();

    if (progetti.length === 0) {
        container.append(`
            <div class="col-12">
                <div class="alert alert-info text-center">
                    <i class="fas fa-info-circle fa-2x mb-3"></i>
                    <p class="mb-0">Nessun progetto disponibile per la fatturazione</p>
                </div>
            </div>
        `);
        return;
    }

    progetti.forEach(progetto => {
        const card = createProgettoCard(progetto);
        container.append(card);
    });
}

function createProgettoCard(progetto) {
    const hasUltimaFattura = progetto.ultimaFattura != null;
    const displayUltimaFattura = hasUltimaFattura ? 'block' : 'none';
    const numeroUltimaFattura = hasUltimaFattura ? progetto.ultimaFattura.numeroFattura : '';
    const dataUltimaFattura = hasUltimaFattura ? formatDate(progetto.ultimaFattura.dataEmissione) : '';

    return `
        <div class="col-md-6 col-lg-4 mb-4" data-progetto-id="${progetto.id}">
            <div class="card shadow-sm h-100 hover-card">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-start mb-3">
                        <h5 class="card-title mb-0 fw-bold">
                            <i class="fas fa-project-diagram text-primary me-2"></i>
                            ${progetto.nome}
                        </h5>
                        <span class="badge bg-info">Attivo</span>
                    </div>

                    <p class="text-muted mb-2">
                        <i class="fas fa-building me-2"></i>
                        <strong>Cliente:</strong> ${progetto.cliente}
                    </p>

                    <p class="text-muted mb-2">
                        <i class="fas fa-calendar me-2"></i>
                        <strong>Periodo:</strong> ${formatDate(progetto.dataInizio)} - ${formatDate(progetto.dataScadenza)}
                    </p>

                    <hr>

                    <div class="row text-center mb-3">
                        <div class="col-6">
                            <div class="border-end">
                                <h3 class="mb-0 text-primary fw-bold">${progetto.oreTotali.toFixed(1)}</h3>
                                <small class="text-muted">Ore Rendicontate</small>
                            </div>
                        </div>
                        <div class="col-6">
                            <h3 class="mb-0 text-success fw-bold">${progetto.numeroFatture}</h3>
                            <small class="text-muted">Fatture Emesse</small>
                        </div>
                    </div>

                    ${progetto.oreTotali > 0 ? `
                        <div class="d-grid gap-2">
                            <button type="button" class="btn btn-primary" onclick="apriFatturazione(${progetto.id})">
                                <i class="fas fa-file-invoice-dollar me-2"></i>
                                Genera Fattura
                            </button>
                            
                            <button type="button" class="btn btn-outline-secondary btn-sm" onclick="visualizzaDettaglio(${progetto.id})">
                                <i class="fas fa-info-circle me-2"></i>
                                Dettaglio Rendicontazioni
                            </button>
                        </div>
                    ` : `
                        <div class="alert alert-warning mb-0 small">
                            <i class="fas fa-exclamation-triangle me-1"></i>
                            Nessuna ora rendicontata ancora
                        </div>
                    `}

                    ${hasUltimaFattura ? `
                        <div class="mt-3">
                            <div class="alert alert-success mb-0 py-2 small">
                                <i class="fas fa-check-circle me-1"></i>
                                <strong>Ultima fattura:</strong> ${numeroUltimaFattura} - ${dataUltimaFattura}
                            </div>
                        </div>
                    ` : ''}
                </div>
            </div>
        </div>
    `;
}

// ===== APERTURA MODALE FATTURAZIONE =====
async function apriFatturazione(progettoId) {
    progettoSelezionato = progettoId;
    
    // Reset form
    $('#input-costo-orario').val('');
    $('#input-iva').val('22');
    $('#input-note').val('');
    $('#preview-container').addClass('d-none');
    $('#preview-placeholder').removeClass('d-none');
    previewData = null;

    try {
        // Carica dettaglio progetto
        const response = await fetch(`/Responsabile/Fatturazione/GetDettaglioProgetto?progettoId=${progettoId}`);
        const data = await response.json();

        // Popola campi modale
        $('#modal-progetto').val(data.nomeProgetto);
        $('#modal-cliente').val(data.cliente);
        $('#modal-periodo').val(`${formatDate(data.periodoDa)} - ${formatDate(data.periodoA)}`);
        $('#modal-ore-totali').val(`${data.oreTotali.toFixed(2)} ore`);

        // Mostra dettaglio dipendenti (opzionale)
        renderDettaglioDipendenti(data.dettaglioDipendenti);

        // Mostra modale
        const modal = new bootstrap.Modal(document.getElementById('modalFatturazione'));
        modal.show();
    } catch (error) {
        console.error('Errore:', error);
        showError('Errore nel caricamento dei dati del progetto');
    }
}

function renderDettaglioDipendenti(dipendenti) {
    // Opzionale: mostra breakdown per dipendente nella modale
    console.log('Dettaglio dipendenti:', dipendenti);
}

// ===== GENERA PREVIEW FATTURA =====
async function generaPreview() {
    const costoOrario = parseFloat($('#input-costo-orario').val());
    const percentualeIva = parseFloat($('#input-iva').val());
    const note = $('#input-note').val().trim();

    // Validazione
    if (!costoOrario || costoOrario <= 0) {
        showError('Inserisci un costo orario valido');
        $('#input-costo-orario').focus();
        return;
    }

    if (costoOrario > 1000) {
        if (!confirm('Il costo orario inserito (€' + costoOrario + ') è molto alto. Confermi?')) {
            return;
        }
    }

    // Mostra loading
    showLoading('Generazione preview in corso...');

    try {
        const requestData = {
            progettoId: progettoSelezionato,
            costoOrario: costoOrario,
            percentualeIva: percentualeIva,
            note: note
        };

        const response = await fetch('/Responsabile/Fatturazione/GeneraPreview', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });

        if (!response.ok) {
            throw new Error('Errore nella generazione della preview');
        }

        const data = await response.json();
        previewData = data;
        renderPreview(data);
        hideLoading();
    } catch (error) {
        console.error('Errore:', error);
        hideLoading();
        showError('Errore nella generazione della preview');
    }
}

function renderPreview(data) {
    // Popola preview fattura
    $('#preview-numero').text(data.numeroFattura);
    $('#preview-data').text(formatDate(data.dataEmissione));
    $('#preview-cliente-nome').text(data.cliente);
    $('#preview-progetto').text(data.nomeProgetto);
    $('#preview-periodo-preview').text(`${formatDate(data.periodoDa)} - ${formatDate(data.periodoA)}`);
    $('#preview-ore').text(data.oreTotali.toFixed(2));
    $('#preview-costo-orario').text(`€ ${data.costoOrario.toFixed(2)}`);
    $('#preview-imponibile').text(`€ ${data.importoImponibile.toFixed(2)}`);
    $('#preview-totale-imponibile').text(`€ ${data.importoImponibile.toFixed(2)}`);
    $('#preview-perc-iva').text(data.percentualeIva.toFixed(0));
    $('#preview-importo-iva').text(`€ ${data.importoIva.toFixed(2)}`);
    $('#preview-totale-finale').text(`€ ${data.importoTotale.toFixed(2)}`);

    // Mostra/nascondi note
    if (data.note) {
        $('#preview-note').text(data.note);
        $('#preview-note-container').show();
    } else {
        $('#preview-note-container').hide();
    }

    // Mostra preview, nascondi placeholder
    $('#preview-placeholder').addClass('d-none');
    $('#preview-container').removeClass('d-none');

    // Scroll to preview (smooth)
    $('#preview-container')[0].scrollIntoView({ behavior: 'smooth', block: 'nearest' });
}

// ===== INVIA FATTURA AL CLIENTE =====
async function inviaFattura() {
    if (!previewData) {
        showError('Genera prima la preview della fattura');
        return;
    }

    // Conferma invio
    const confirmMessage = `
        Confermi l'invio della fattura?
        
        N° Fattura: ${previewData.numeroFattura}
        Cliente: ${previewData.cliente}
        Importo: € ${previewData.importoTotale.toFixed(2)}
    `;

    if (!confirm(confirmMessage)) {
        return;
    }

    // Mostra loading
    showLoading('Invio fattura in corso...');

    try {
        const requestData = {
            progettoId: progettoSelezionato,
            costoOrario: parseFloat($('#input-costo-orario').val()),
            percentualeIva: parseFloat($('#input-iva').val()),
            note: $('#input-note').val().trim()
        };

        const response = await fetch('/Responsabile/Fatturazione/InviaFattura', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });

        if (!response.ok) {
            throw new Error('Errore nell\'invio della fattura');
        }

        const data = await response.json();

        if (data.success) {
            hideLoading();
            
            // Chiudi modale
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalFatturazione'));
            modal.hide();

            // Mostra successo
            showSuccess(`Fattura ${data.numeroFattura} inviata con successo al cliente!`);

            // Ricarica progetti
            loadProgetti();

            // Opzionale: apri tab fatture
            // $('#fatture-tab').tab('show');
            // loadFatture();
        } else {
            throw new Error(data.message || 'Errore nell\'invio della fattura');
        }
    } catch (error) {
        console.error('Errore:', error);
        hideLoading();
        showError(error.message);
    }
}

// ===== VISUALIZZA DETTAGLIO PROGETTO =====
async function visualizzaDettaglio(progettoId) {
    try {
        const response = await fetch(`/Responsabile/Fatturazione/GetDettaglioProgetto?progettoId=${progettoId}`);
        const data = await response.json();

        // Crea modale con dettaglio
        let html = `
            <div class="modal fade" id="modalDettaglio" tabindex="-1">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Dettaglio Rendicontazioni - ${data.nomeProgetto}</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <p><strong>Cliente:</strong> ${data.cliente}</p>
                            <p><strong>Periodo:</strong> ${formatDate(data.periodoDa)} - ${formatDate(data.periodoA)}</p>
                            <p><strong>Ore Totali:</strong> ${data.oreTotali.toFixed(2)} ore</p>
                            
                            <hr>
                            
                            <h6>Breakdown per Dipendente:</h6>
                            <table class="table table-sm">
                                <thead>
                                    <tr>
                                        <th>Dipendente</th>
                                        <th class="text-end">Ore</th>
                                        <th class="text-end">Attività</th>
                                        <th>Periodo</th>
                                    </tr>
                                </thead>
                                <tbody>
        `;

        data.dettaglioDipendenti.forEach(dip => {
            html += `
                <tr>
                    <td>${dip.nomeDipendente}</td>
                    <td class="text-end fw-bold">${dip.oreTotali.toFixed(1)}h</td>
                    <td class="text-end">${dip.numeroAttivita}</td>
                    <td class="small">${formatDate(dip.periodoDa)} - ${formatDate(dip.periodoA)}</td>
                </tr>
            `;
        });

        html += `
                                </tbody>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Chiudi</button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        // Rimuovi modale precedente se esiste
        $('#modalDettaglio').remove();

        // Aggiungi e mostra
        $('body').append(html);
        const modal = new bootstrap.Modal(document.getElementById('modalDettaglio'));
        modal.show();

        // Cleanup on hide
        $('#modalDettaglio').on('hidden.bs.modal', function () {
            $(this).remove();
        });

    } catch (error) {
        console.error('Errore:', error);
        showError('Errore nel caricamento del dettaglio');
    }
}

// ===== CARICAMENTO FATTURE EMESSE =====
async function loadFatture() {
    try {
        const response = await fetch('/Responsabile/Fatturazione/GetFatture');
        const fatture = await response.json();
        renderFatture(fatture);
    } catch (error) {
        console.error('Errore:', error);
        showError('Errore nel caricamento delle fatture');
    }
}

function renderFatture(fatture) {
    const tbody = $('#fatture-table tbody');
    tbody.empty();

    if (fatture.length === 0) {
        tbody.append(`
            <tr>
                <td colspan="8" class="text-center text-muted py-4">
                    <i class="fas fa-inbox fa-2x mb-2 d-block"></i>
                    Nessuna fattura emessa
                </td>
            </tr>
        `);
        return;
    }

    fatture.forEach(f => {
        const statoClass = {
            'Pagata': 'success',
            'Inviata': 'primary',
            'Bozza': 'secondary',
            'Annullata': 'danger'
        }[f.stato] || 'secondary';

        const row = `
            <tr>
                <td><strong>${f.numeroFattura}</strong></td>
                <td>${formatDate(f.dataEmissione)}</td>
                <td>${f.cliente}</td>
                <td>${f.nomeProgetto}</td>
                <td>${f.oreTotali.toFixed(1)}h</td>
                <td class="fw-bold text-success">€ ${f.importoTotaleConIva.toFixed(2)}</td>
                <td><span class="badge bg-${statoClass}">${f.stato}</span></td>
                <td>
                    <button class="btn btn-sm btn-outline-primary" onclick="visualizzaFattura(${f.id})" title="Visualizza">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-secondary" onclick="scaricaFatturaPDF(${f.id})" title="Scarica PDF">
                        <i class="fas fa-download"></i>
                    </button>
                </td>
            </tr>
        `;
        tbody.append(row);
    });
}

// ===== VISUALIZZA FATTURA =====
function visualizzaFattura(id) {
    // TODO: Implementare visualizzazione fattura singola
    showInfo('Funzionalità di visualizzazione fattura da implementare');
}

// ===== SCARICA PDF FATTURA =====
function scaricaFatturaPDF(id) {
    // TODO: Implementare download PDF
    showInfo('Funzionalità di download PDF da implementare');
}

// ===== UTILITY FUNCTIONS =====
function formatDate(dateString) {
    if (!dateString) return 'N/D';
    const date = new Date(dateString);
    return date.toLocaleDateString('it-IT', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}

function showLoading(message = 'Caricamento...') {
    // Usa Toast di Bootstrap o spinner
    console.log('Loading:', message);
    // TODO: Implementare spinner/loading overlay
}

function hideLoading() {
    console.log('Hide loading');
    // TODO: Nascondere spinner
}

function showSuccess(message) {
    alert('✅ ' + message);
    // TODO: Usare toast/alert più elegante
}

function showError(message) {
    alert('❌ ' + message);
    // TODO: Usare toast/alert più elegante
}

function showInfo(message) {
    alert('ℹ️ ' + message);
    // TODO: Usare toast/alert più elegante
}