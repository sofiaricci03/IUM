# 💰 Fatturazione Clienti - Guida Responsabile

## Panoramica
Il modulo **Fatturazione** permette di generare fatture per i clienti basate sulle ore lavorate e approvate dai dipendenti, con calcolo automatico di imponibile, IVA e totali.

---

## Come Accedere
1. Login con credenziali Responsabile
2. Nella **sidebar**, clicca su **"Fatturazione"** (icona scontrino)
3. Vedrai due tab: "Progetti da Fatturare" e "Fatture Emesse"

**URL diretto**: `http://localhost:5178/Responsabile/Fatturazione`

---

## Concetti Fondamentali

### Cos'è una Fattura?
Una fattura è un documento fiscale che addebita al cliente il costo delle **ore lavorate** su un progetto, calcolato come:

```
FATTURA = Ore Approvate × Costo Orario + IVA
```

### Prerequisiti per Fatturare
Prima di poter fatturare un progetto, devono verificarsi:

1. ✅ **Progetto creato** e dipendenti assegnati
2. ✅ **Dipendenti hanno registrato ore** nel calendario
3. ✅ **Dipendenti hanno inviato rendicontazioni mensili**
4. ✅ **Tu hai APPROVATO le rendicontazioni**

**⚠️ IMPORTANTE**: Solo le **ore approvate** diventano fatturabili!

### Flusso Completo

```
CREAZIONE PROGETTO
    ↓
ASSEGNAZIONE DIPENDENTI
    ↓
DIPENDENTI: Registrano ore quotidiane
    ↓
FINE MESE
    ↓
DIPENDENTI: Inviano rendicontazioni
    ↓
RESPONSABILE: Approva rendicontazioni
    ↓
📊 ORE DIVENTANO FATTURABILI
    ↓
RESPONSABILE: Va su "Fatturazione"
    ↓
RESPONSABILE: Genera fattura per progetto
    ↓
📄 FATTURA INVIATA AL CLIENTE
```

---

## Interfaccia Principale

### Tab 1: Progetti da Fatturare

Mostra **card progetti** con:
- **Nome Progetto**
- **Cliente**
- **Periodo** (date attività)
- **Ore Totali** rendicontate e approvate
- **Numero Fatture** già emesse per quel progetto
- **Ultima Fattura** (se esiste)
- Pulsanti:
  - **"Genera Fattura"**: Apre modale fatturazione
  - **"Dettaglio"**: Mostra breakdown ore per dipendente

**Esempio di card progetto**:
```
┌─────────────────────────────────────┐
│ 📊 Sistema ERP Aziendale            │
│ 🏢 Acme Corporation                 │
├─────────────────────────────────────┤
│ 📅 01/01/2026 - 31/01/2026          │
│                                     │
│ ⏱️  80.5 ore                         │
│ 📄  2 Fatture                        │
│                                     │
│ ✅ Ultima: 2026/002 - 15/02/2026    │
│                                     │
│ [Genera Fattura] [Dettaglio]       │
└─────────────────────────────────────┘
```

### Tab 2: Fatture Emesse

Tabella con tutte le fatture emesse:
- **N° Fattura** (es: 2026/001)
- **Data Emissione**
- **Cliente**
- **Progetto**
- **Ore** fatturate
- **Importo Totale** (con IVA)
- **Stato**: Inviata / Pagata / Annullata
- **Azioni**: Visualizza, Scarica PDF (se disponibile)

---

## Generare una Fattura

### Step-by-Step

#### Step 1: Seleziona Progetto
1. Vai sul tab **"Progetti da Fatturare"**
2. Identifica il progetto da fatturare (con ore > 0)
3. Clicca su **"Genera Fattura"**

#### Step 2: Verifica Dati Progetto
Il modale si apre e mostra:
- **Nome Progetto** (readonly)
- **Cliente** (readonly)
- **Periodo** (readonly) - Basato sulle date delle attività
- **Ore Totali** (readonly) - Solo ore approvate

**Esempio**:
```
Progetto: Sistema ERP Aziendale
Cliente: Acme Corporation
Periodo: 01/01/2026 - 31/01/2026
Ore Totali: 80.50 ore
```

#### Step 3: Inserisci Parametri Fatturazione
**Costo Orario** ⭐ (Obbligatorio):
- Inserisci la tariffa oraria per questo cliente
- Esempio: €75.00 / ora
- **Importante**: Può variare per cliente/progetto

**IVA %**:
- Seleziona percentuale IVA:
  - 0% - Esente (es: regime forfettario)
  - 4% - Ridotta (es: alcuni servizi)
  - 10% - Ridotta
  - 22% - Ordinaria (default)

**Note** (Opzionale):
- Aggiungi note che appariranno sulla fattura
- Esempio: "Fattura per servizi di consulenza tecnica"

**Esempio compilazione**:
```
Costo Orario: €75.00
IVA: 22%
Note: Servizi di sviluppo software mese Gennaio 2026
```

#### Step 4: Genera Preview
1. Clicca su **"Genera Preview"**
2. Il sistema calcola automaticamente:
   - **Numero Fattura**: Progressivo annuale (es: 2026/003)
   - **Data Emissione**: Oggi
   - **Imponibile**: Ore × Costo Orario
   - **Importo IVA**: Imponibile × (IVA% / 100)
   - **Totale Fattura**: Imponibile + IVA

**Esempio di calcolo**:
```
Ore Totali:        80.50 h
Costo Orario:      €75.00
─────────────────────────
Imponibile:        €6,037.50
IVA (22%):         €1,328.25
─────────────────────────
TOTALE FATTURA:    €7,365.75
```

#### Step 5: Visualizza Preview
La preview mostra una **fattura formattata** con:

**Intestazione**:
- Logo azienda (se configurato)
- Numero fattura: **2026/003**
- Data emissione: **03/02/2026**

**Dati Fornitore**:
```
La Tua Azienda S.r.l.
Via Example 123
40100 Bologna (BO)
P.IVA: IT12345678901
```

**Dati Cliente**:
```
Acme Corporation
```

**Corpo Fattura**:
```
Progetto: Sistema ERP Aziendale
Periodo: 01/01/2026 - 31/01/2026

┌────────────────────────┬──────┬────────┬──────────┐
│ Descrizione            │ Ore  │ €/h    │ Totale   │
├────────────────────────┼──────┼────────┼──────────┤
│ Attività di sviluppo   │ 80.5 │ 75.00  │ 6,037.50 │
│ software               │      │        │          │
└────────────────────────┴──────┴────────┴──────────┘

Imponibile:              €6,037.50
IVA (22%):               €1,328.25
═══════════════════════════════════
TOTALE FATTURA:          €7,365.75
═══════════════════════════════════

Note: Servizi di sviluppo software mese Gennaio 2026
```

#### Step 6: Invia al Cliente
1. Verifica che tutto sia corretto
2. Clicca su **"Invia al Cliente"** (pulsante verde grande)
3. Conferma l'invio
4. Il sistema:
   - **Salva la fattura** nel database
   - Assegna numero progressivo definitivo
   - Imposta stato "Inviata"
   - Chiude il modale

**✅ Fattura Creata!**

---

## Numerazione Automatica Fatture

### Sistema di Numerazione

Le fatture seguono il formato: **ANNO/PROGRESSIVO**

**Esempi**:
```
Prima fattura 2026: 2026/001
Seconda fattura 2026: 2026/002
Terza fattura 2026: 2026/003
...
Ultima fattura 2026: 2026/156

Prima fattura 2027: 2027/001 (ripartei da 1)
```

### Caratteristiche

✅ **Progressivo automatico**: Non devi inserire il numero manualmente  
✅ **Per anno**: Ogni anno riparte da 001  
✅ **Sequenziale**: Nessun "buco" nella numerazione  
✅ **Unico**: Un numero = una fattura  

**⚠️ Attenzione**: I numeri non sono modificabili dopo l'emissione!

---

## Gestire le Fatture Emesse

### Visualizzare Fatture

**Tab "Fatture Emesse"**:
- Tabella ordinata per data (più recenti prima)
- Filtri per:
  - Cliente
  - Progetto
  - Periodo
  - Stato

### Stati Fattura

| Stato | Badge | Significato |
|-------|-------|-------------|
| **Inviata** | 🔵 Blu | Inviata al cliente, in attesa pagamento |
| **Pagata** | 🟢 Verde | Cliente ha pagato |
| **Annullata** | 🔴 Rosso | Fattura annullata (errore/storno) |

**Cambio Stato** (feature futura):
- Segna come "Pagata" quando il cliente paga
- Annulla se necessario storno

### Azioni su Fattura

**Visualizza**:
- Mostra dettagli completi
- Calcoli eseguiti
- Note e periodo

**Scarica PDF** (se disponibile):
- Download fattura in formato PDF
- Pronta per invio email o stampa

---

## Scenari Comuni

### Scenario 1: Fatturazione Mensile Standard

**Situazione**: Cliente Acme - Progetto ERP - Fatturazione mensile

**Timeline**:
```
GENNAIO 2026
│
├─ Dipendenti lavorano quotidianamente
│  - Mario: 60 ore
│  - Luca: 45 ore
│  - Giulia: 35 ore
│  TOTALE: 140 ore
│
└─ 31 Gennaio: Fine mese

INIZIO FEBBRAIO
│
├─ 1 Feb: Dipendenti inviano rendicontazioni
│
├─ 3 Feb: Responsabile approva tutte
│         ✅ 140 ore diventano fatturabili
│
└─ 5 Feb: Responsabile genera fattura
          1. Va su "Fatturazione"
          2. Seleziona progetto ERP
          3. Costo orario: €80/h
          4. IVA: 22%
          5. Genera preview
          6. Calcolo:
             140h × €80 = €11,200 + 22% IVA
             = €13,664 totale
          7. Invia fattura 2026/005

✅ Fattura inviata ad Acme per €13,664
```

### Scenario 2: Progetto con Tariffa Variabile

**Situazione**: Hai 2 progetti per lo stesso cliente, tariffe diverse

**Progetti**:
```
1. ERP Development (tariffa senior): €100/h
2. Support & Maintenance (tariffa junior): €50/h
```

**Come fatturare**:
- Fattura **separata** per ogni progetto
- Usa la tariffa corretta per ciascuno

**Esempio**:
```
Fattura 2026/010: ERP Development
- 80 ore × €100 = €8,000 + IVA
- Totale: €9,760

Fattura 2026/011: Support & Maintenance
- 120 ore × €50 = €6,000 + IVA
- Totale: €7,320

TOTALE DA PAGARE: €17,080
```

### Scenario 3: Fatturazione Trimestrale

**Situazione**: Cliente preferisce fatturazione trimestrale

**Approccio**:
```
Q1 2026 (Gen-Mar)
│
├─ Gennaio: 80 ore registrate e approvate
├─ Febbraio: 75 ore registrate e approvate
└─ Marzo: 90 ore registrate e approvate

1 Aprile: Genera fattura trimestrale
- Ore Totali: 80 + 75 + 90 = 245 ore
- Tariffa: €75/h
- Imponibile: 245 × €75 = €18,375
- IVA 22%: €4,042.50
- Totale: €22,417.50

✅ Fattura 2026/012 per Q1 2026
```

**Nota**: Il sistema raggruppa automaticamente tutte le ore del periodo selezionato.

### Scenario 4: Correzione Fattura

**Situazione**: Hai emesso una fattura con tariffa errata

**Problema**:
```
Fattura 2026/008 emessa:
- 100 ore × €70/h (ERRATO)
- Totale: €8,540

Tariffa corretta: €80/h
```

**Soluzione**:
1. **NON eliminare** la fattura 2026/008
2. **Emetti nota di credito** (storno):
   - Fattura -2026/008 per -€8,540
3. **Emetti nuova fattura** corretta:
   - Fattura 2026/009: 100h × €80 = €9,760

**Risultato**:
```
2026/008: €8,540 (annullata con nota credito)
2026/008NC: -€8,540 (nota di credito)
2026/009: €9,760 (corretta)

Cliente paga: €9,760
```

---

## Dettaglio Ore per Dipendente

### Visualizzare Breakdown

1. Nella card progetto, clicca **"Dettaglio"**
2. Si apre modale con **tabella** ore per dipendente:

**Esempio**:
```
Progetto: Sistema ERP
Cliente: Acme Corporation
Ore Totali: 140 ore

┌───────────────┬──────┬─────────┬────────────┐
│ Dipendente    │ Ore  │ Attività│ Periodo    │
├───────────────┼──────┼─────────┼────────────┤
│ Mario Verdi   │ 60.0 │ 24      │ 5-31 Gen   │
│ Luca Neri     │ 45.5 │ 18      │ 10-30 Gen  │
│ Giulia Bianchi│ 34.5 │ 15      │ 8-28 Gen   │
└───────────────┴──────┴─────────┴────────────┘
```

### Utilizzo

**Quando visualizzare**:
- Prima di fatturare, per verificare distribuzione ore
- Per audit e controlli
- Per report interni

**Informazioni utili**:
- Chi ha lavorato di più
- Numero di attività registrate
- Periodo di lavoro

---

## Calcoli e Formule

### Calcolo Imponibile

```
Imponibile = Ore Totali × Costo Orario

Esempio:
80.5 ore × €75.00 = €6,037.50
```

### Calcolo IVA

```
Importo IVA = Imponibile × (Percentuale IVA / 100)

Esempio:
€6,037.50 × (22 / 100) = €6,037.50 × 0.22 = €1,328.25
```

### Calcolo Totale Fattura

```
Totale Fattura = Imponibile + Importo IVA

Esempio:
€6,037.50 + €1,328.25 = €7,365.75
```

### Verifica Calcoli

**Controllo manuale**:
```
1. Conta le ore: 80.50
2. Moltiplica per tariffa: 80.50 × €75 = €6,037.50 ✅
3. Calcola IVA: €6,037.50 × 22% = €1,328.25 ✅
4. Somma: €6,037.50 + €1,328.25 = €7,365.75 ✅
```

---

## Best Practices

### ✅ Cosa Fare

1. **Approva rendicontazioni PRIMA di fatturare**
   - Solo ore approvate sono fatturabili
   - Controlla accuratezza prima di approvare

2. **Verifica calcoli nella preview**
   - Controlla ore totali
   - Verifica tariffa oraria
   - Controlla IVA

3. **Usa tariffe coerenti**
   - Mantieni tariffe consistenti per cliente
   - Documenta variazioni (senior/junior)

4. **Fattura regolarmente**
   - Mensile: Più cash flow
   - Trimestrale: Meno amministrazione
   - Scegli e mantieni

5. **Aggiungi note utili**
   - Riferimenti contratto
   - Periodo fatturato
   - Dettagli servizio

6. **Archivia fatture emesse**
   - Scarica PDF quando disponibile
   - Backup regolare

### ❌ Cosa NON Fare

1. **Non fatturare senza approvare**
   - Ore non approvate = €0
   - Approva prima

2. **Non modificare fatture emesse**
   - I numeri sono definitivi
   - Usa note di credito per correzioni

3. **Non confondere tariffe**
   - Verifica sempre prima di inviare
   - Errori sono costosi da correggere

4. **Non fatturare troppo tardi**
   - Contratti spesso prevedono termini
   - Cash flow ne risente

5. **Non dimenticare IVA**
   - Seleziona sempre percentuale corretta
   - Verifica regime cliente (es: reverse charge)

---

## Reportistica e Analytics

### KPI Principali

**Per Periodo**:
```
Fatture Emesse: 15
Importo Totale: €125,000
IVA Incassata: €27,500
Imponibile: €97,500

Ore Fatturate: 1,300 ore
Tariffa Media: €75/h
```

**Per Cliente**:
```
Cliente: Acme Corporation
Fatture 2026: 6
Importo Totale: €45,000
Ore Fatturate: 600 ore
```

**Per Progetto**:
```
Progetto: Sistema ERP
Fatture: 4
Ore Totali: 320 ore
Ricavi: €24,000
```

### Come Ottenere Report

**Manuale**:
1. Vai su tab "Fatture Emesse"
2. Filtra per periodo/cliente/progetto
3. Esporta in Excel/CSV (se disponibile)

**Automatico** (feature futura):
- Dashboard analytics
- Grafici fatturato
- Export automatizzati

---

## Integrazioni

### Con Rendicontazione

```
RENDICONTAZIONE → FATTURAZIONE

1. Dipendente invia rendicontazione
2. Responsabile approva
3. Ore approvate → Diventano fatturabili
4. Responsabile genera fattura
5. Ore fatturate → Tracciamento completato
```

### Con Progetti

```
PROGETTI → FATTURAZIONE

1. Crea progetto "ERP Development"
2. Assegna dipendenti
3. Dipendenti lavorano e registrano ore
4. Ore approvate accumulate
5. Genera fattura per "ERP Development"
6. Fattura associata al progetto
7. Storico fatture per progetto
```

---

## FAQ

**Q: Posso fatturare ore non ancora approvate?**  
A: **No**, solo ore con rendicontazione approvata sono fatturabili.

**Q: Cosa succede se approvo rendicontazioni dopo aver fatturato?**  
A: Le nuove ore approvate saranno disponibili per la prossima fattura.

**Q: Posso modificare una fattura dopo averla inviata?**  
A: **No**, le fatture emesse non sono modificabili. Usa note di credito per correzioni.

**Q: Posso fatturare più volte lo stesso progetto?**  
A: **Sì**, puoi emettere più fatture per lo stesso progetto (es: fatturazione mensile).

**Q: Come gestisco sconti?**  
A: Riduci il costo orario oppure aggiungi nota con sconto applicato manualmente.

**Q: Come funziona con clienti esteri (reverse charge)?**  
A: Seleziona IVA 0% e aggiungi nota "Reverse charge art. XY".

**Q: Posso cancellare una fattura?**  
A: **No**, segna come "Annullata" ed emetti nota di credito.

**Q: Come traccio i pagamenti?**  
A: (Feature futura) Segna fattura come "Pagata" con data pagamento.

---

## Roadmap Feature Futuri

### Fase 2 (Q2 2026)
- ✨ Export fatture in **PDF** automatico
- ✨ Invio fatture via **email** al cliente
- ✨ Note di **credito** integrate

### Fase 3 (Q3 2026)
- 📊 Dashboard **analytics** fatturato
- 📈 Grafici **ricavi** mensili
- 📉 Report **DSO** (Days Sales Outstanding)
- 💰 Tracciamento **pagamenti** con scadenze

### Fase 4 (Q4 2026)
- 🔔 **Alerting** automatico per scadenze
- 📧 Solleciti pagamenti automatici
- 🧾 Integrazione con **contabilità** (es: XML fatture elettroniche)
- 🌐 Multi-valuta (USD, EUR, GBP)

---

## Checklist Prima di Fatturare

Prima di inviare una fattura, verifica:

- [ ] Tutte le rendicontazioni del periodo sono state approvate
- [ ] Le ore totali corrispondono alle aspettative
- [ ] La tariffa oraria è corretta per questo cliente/progetto
- [ ] La percentuale IVA è corretta
- [ ] Hai controllato la preview
- [ ] I calcoli sono corretti (imponibile, IVA, totale)
- [ ] Le note sono chiare e complete
- [ ] Il cliente è corretto
- [ ] Il periodo fatturato è corretto

✅ **Se tutti i punti sono verificati, puoi inviare!**

---

## Supporto
Per assistenza:
- 📧 Email: admin@azienda.it
- 📞 Telefono: +39 051 1234567
- 💬 Chat: #supporto-fatturazione

---

**Ultima modifica**: Febbraio 2026  
**Versione**: 1.0 - Feature Lancio
