### Caso di Studio: Sistema di Rendicontazione e Fatturazione Attività
Laboratorio di Interfaccia Uomo-Macchina 

## Nascita dell'Esigenza

Contesto Aziendale

Un'azienda di consulenza IT con forte crescita nel settore dei servizi a valore ha evidenziato criticità nella gestione delle ore lavorate e nella conseguente fatturazione ai clienti. Questa necessità ha portato alla nascita del progetto **"Rendicontazione Attività"**.

# Obiettivo del Progetto
Il progetto prevede lo sviluppo di una piattaforma software che permetta di:

- Tracciare in maniera precisa le ore lavorate dai dipendenti
- Rendicontare mensilmente le attività svolte
- Approvare/respingere le rendicontazioni con feedback strutturato
- Fatturare automaticamente i clienti basandosi sulle ore approvate


## Analisi del Contesto

Struttura Aziendale

L'azienda è una realtà che conta circa 50 dipendenti organizzati in:

Area Development (20 persone)
Area Design & UX (8 persone)
Area QA & Testing (7 persone)
Area Project Management (5 persone)
Area Amministrativa (10 persone)

Modello di Business
Ogni area lavora su progetti per clienti esterni con modello:

Time & Materials (T&M): Fatturazione basata su ore lavorate
Tariffe orarie differenziate per cliente/progetto
SLA contrattuali da rispettare
Necessità di tracciabilità completa per audit e conformità


## Flusso Base Dipendente

Ciclo Mensile Standard

Dipendente registra quotidianamente le attività nel calendario
Il sistema salva automaticamente ore, progetto e descrizione
A fine mese il dipendente crea la rendicontazione
Il sistema aggrega automaticamente tutte le attività del mese
Il dipendente verifica il riepilogo ore per progetto
Il dipendente invia la rendicontazione al Responsabile
Stato diventa "In Attesa" di approvazione
Il Responsabile riceve notifica della rendicontazione da esaminare
Il dipendente riceve notifica di approvazione/rifiuto
Se approvata, le ore diventano fatturabili
Se respinta, il dipendente deve correggere e reinviare


## Interviste: Pain Points Emersi

Interviste condotte con 11 persone (8 dipendenti + 3 responsabili)
Durata interviste: 3 settimane (Gennaio 2026)
Problematiche Dipendenti

Dimenticanza frequente registrazione ore giornaliere
Difficoltà a ricordare progetti su cui si è lavorato giorni prima
Tempo perso a compilare rendicontazioni manualmente (Excel)
Incertezza sullo stato delle rendicontazioni (approvate/respinte?)
Mancanza di visibilità sui progetti assegnati
Confusione su quali progetti possono registrare ore


## Interviste: Pain Points Responsabili
Criticità Emerse

Volume elevato di rendicontazioni da esaminare (10-15 al mese)
Tempo eccessivo per controlli manuali (3-4 ore totali)
Errori frequenti: ore su progetti sbagliati, ore duplicate, descrizioni poco chiare
Processo fatturazione manuale estremamente time-consuming (30 min per fattura)
Calcoli manuali con alto rischio di errore (imponibile, IVA, totali)
Mancanza di feedback strutturato per rifiuti (comunicazione via email inefficiente)
Scarsa visibilità d'insieme su distribuzione ore team

Quote Significative

"Devo incrociare ore approvate con tariffe cliente, calcolare IVA... è un incubo che richiede 2 ore ogni mese solo per fatturare" - Anna, Project Manager


## Service Blueprint: Registrazione Attività
Front Stage (Azioni Utente Visibili)
Login → Visualizza Calendario → Clicca Giorno → 
Modale "Registra Attività" → Seleziona Progetto (dropdown) → 
Inserisce Descrizione → Ora Inizio/Fine → 
Sistema CALCOLA ore automaticamente → Salva
On Stage (Interazioni Sistema Visibili)
Calendario colorato (Verde = attività, Bianco = vuoto) →
Dropdown con SOLO progetti assegnati →
Calcolo automatico: Ora Fine - Ora Inizio = Ore Totali →
Conferma visiva: Giorno diventa Verde
Back Stage (Processi Non Visibili)
Query database: SELECT progetti WHERE DipendenteId = current_user →
Validazione: Progetto assegnato? Ore > 0? Date valide? →
Salvataggio: INSERT AttivitaLavorativa →
Timestamp creazione
Support Processes
Database: AttivitaLavorative, AssegnazioniProgetti, Progetti 

## Service Blueprint: Fatturazione

# Front Stage (Responsabile)
Rendicontazione → Approva tutte del mese → 
Fatturazione → Visualizza "Progetti da Fatturare" →
Seleziona Progetto → Inserisce Tariffa Oraria e IVA →
"Genera Preview" → Verifica Calcoli →
"Invia al Cliente"
On Stage
Card progetti con ore fatturabili →
Form input: Costo Orario,  Note →
Preview LIVE con calcolo automatico:
  - Imponibile = Ore × Tariffa
Numero fattura PROGRESSIVO automatico (2026/XXX)

# Back Stage
Aggregazione ore approvate per progetto →
Calcolo totali con formule predefinite →
Generazione numero progressivo (query MAX + 1) →
Salvataggio fattura in database →
Update stato ore: "Fatturate"
Support Processes
Algoritmo numerazione progressiva annuale 

## Line of Visibility
Struttura a Layers
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
UTENTE (Dipendente/Responsabile)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
FRONT-END VISIBILE
  - Calendario
  - Form registrazione
  - Dashboard rendicontazione
  - Modale fatturazione
━━━━━━━━━━━━━━━━━━━━━━━━━ LINE OF VISIBILITY ━━
BACK-END NON VISIBILE
  - Validazioni business logic
  - Query SQL complesse
  - Algoritmi calcolo (ore, IVA, progressivi)
  - Sistema notifiche
━━━━━━━━━━━━━━━━━━━━━━ LINE OF INTERACTION ━━━
SUPPORT SYSTEMS
  - Database SQL Server
  - Sistema email (SMTP)
  - Audit log
  - Backup automatici
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

### Milestone di Sviluppo
Roadmap Biennale (2025-2026)
Queste informazioni raccolte sono state racchiuse a livello di progettazione in 4 grandi macrocapitoli da sviluppare su base annuale:

Registrazione Attività e Rendicontazione Base (Q4 2025) 
Approvazione con Feedback Strutturato (Q1 2026) 
Fatturazione Automatica (Q1 2026) 
Sistema Reminder e Notifiche Intelligenti (Q2 2026) ⏳


### Milestone 1: Registrazione e Rendicontazione
Obiettivo
Permettere ai dipendenti di tracciare quotidianamente le ore lavorate e rendicontare mensilmente in modo automatizzato.
Funzionalità Implementate
Calendario Attività:

Vista mensile con griglia giorni
Click su giorno → Modale registrazione
Selezione progetto da dropdown (solo assegnati)
Calcolo automatico ore (Ora Fine - Ora Inizio)
Colore verde = attività registrate

Rendicontazione Mensile:

Caricamento automatico attività del mese
Aggregazione per progetto
Calcolo totali automatico
Note opzionali
Invio con conferma

Risultato
Sistema base funzionante per tracciamento ore quotidiano con rendicontazione automatizzata.

### Milestone 1: Design Calendario
[./screenshot_app/calendario_attivita.png]

Elementi chiave visibili:

Sidebar sinistra con menu: Attività, Rendicontazione, Congedo, Progetti
Header con statistiche mese (Ore Totali, Giorni Lavorati, Media)
Griglia calendario 7 colonne × 5 righe
Giorni con attività: sfondo verde
Giorni senza attività: sfondo bianco
Giorno corrente: bordo blu
Click su giorno → Modale centrale


Slide 12 - Milestone 1: Modale Registrazione
[MOCKUP: Form Registrazione Attività]
(Tralasciando screenshot)
Form struttura:
┌─────────────────────────────────────┐
│ Registra Attività - 15 Gennaio 2026 │
├─────────────────────────────────────┤
│ Progetto: [Dropdown] ⭐             │
│ ├─ Sistema ERP                      │
│ ├─ CRM Aziendale                    │
│ └─ Sito Web Corporate               │
│                                     │
│ Descrizione: [Textarea] ⭐          │
│ Es: Sviluppo API REST modulo ...   │
│                                     │
│ Ora Inizio: [09:00] ⭐              │
│ Ora Fine:   [13:00] ⭐              │
│                                     │
│ Ore Totali: 4.0 ore (calcolate)    │
│                                     │
│ [Annulla]  [Salva Attività]        │
└─────────────────────────────────────┘
⭐ = Campo obbligatorio

Slide 13 - Milestone 1: Rendicontazione Preview
[MOCKUP: Preview Rendicontazione]
(Tralasciando screenshot)
Struttura informazioni:
Rendicontazione: Gennaio 2026
Dipendente: Mario Verdi
Stato: Bozza

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📁 Sistema ERP (Cliente: Acme Corp)
   05/01: Sviluppo API REST (4.0h)
   06/01: Testing moduli (3.5h)
   10/01: Documentazione (2.5h)
   ...
   Totale Progetto: 60.0 ore

📁 CRM Aziendale (Cliente: Beta Inc)
   07/01: Design UI (6.0h)
   08/01: Implementazione form (5.0h)
   ...
   Totale Progetto: 45.0 ore

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🎯 ORE TOTALI MESE: 105.0 ore

Note: [Campo opzionale per comunicazioni]

[Annulla] [Invia Rendicontazione]

Slide 14 - Milestone 2: Approvazione e Feedback
Obiettivo
Permettere ai Responsabili di esaminare, approvare o respingere rendicontazioni con feedback strutturato.
Funzionalità Implementate
Vista Responsabile:

Lista rendicontazioni con filtro per stato
Badge colorati: 🟡 In Attesa, 🟢 Approvata, 🔴 Respinta
Dettaglio completo con breakdown ore
Pulsanti Approva/Respingi visibili

Approvazione:

Click "Approva" → Conferma → Ore diventano fatturabili
Notifica automatica a dipendente

Rifiuto con Feedback:

Click "Respingi" → Modale con campo motivazione obbligatorio
Template suggeriti per motivazioni comuni
Notifica con motivazione inviata a dipendente

Risultato
Workflow completo di approvazione con tracciabilità e comunicazione efficace.

Slide 15 - Milestone 2: Lista Rendicontazioni
[MOCKUP: Dashboard Responsabile]
(Tralasciando screenshot)
Tabella struttura:
┌────────────────────────────────────────────────────────────┐
│ Rendicontazioni - Filtri: [In Attesa ▼] [Tutti Dipendenti]│
├────────┬──────────┬──────┬────────┬─────────┬─────────────┤
│Dipend. │ Mese     │ Ore  │ Invio  │ Stato   │ Azioni      │
├────────┼──────────┼──────┼────────┼─────────┼─────────────┤
│Mario V.│Gen 2026  │105.0h│05/02   │🟡 Attesa│[Esamina]    │
│Luca N. │Gen 2026  │220.0h│05/02   │🟡 Attesa│[Esamina] ⚠️│
│Anna B. │Gen 2026  │160.0h│04/02   │🟢 Approv│[Visualizza] │
│Giulia R│Gen 2026  │145.0h│06/02   │🟡 Attesa│[Esamina]    │
│Paolo V.│Gen 2026  │ 90.0h│03/02   │🔴 Respnt│[Visualizza] │
└────────┴──────────┴──────┴────────┴─────────┴─────────────┘

⚠️ = Alert automatico "Anomalie rilevate"
Alert automatici:

Ore giornaliere > 12h
Ore mensili > 200h
Ore giornaliere < 2h (giorni lavorativi)


Slide 16 - Milestone 2: Dettaglio e Approvazione
[MOCKUP: Esame Rendicontazione]
(Tralasciando screenshot)
Interfaccia split:
┌─────────────────────────────────────────────────────────┐
│ Rendicontazione: Gennaio 2026 - Mario Verdi            │
│ [Approva ✓]  [Respingi ✗]                             │
├─────────────────────────────────────────────────────────┤
│ Ore Totali: 105.0h                                      │
│ Data Invio: 05/02/2026                                  │
│ Stato: 🟡 In Attesa                                     │
│                                                         │
│ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━  │
│                                                         │
│ 📊 Breakdown per Progetto:                             │
│                                                         │
│ [▼] Sistema ERP - 60.0h                                │
│     05/01: Sviluppo API REST (4.0h)                    │
│     06/01: Testing moduli (3.5h)                       │
│     10/01: Documentazione (2.5h)                       │
│     ...                                                 │
│                                                         │
│ [▶] CRM Aziendale - 45.0h                              │
│                                                         │
│ 📝 Note dipendente:                                    │
│ "Ore extra per deadline progetto ERP 15/01"           │
└─────────────────────────────────────────────────────────┘

Slide 17 - Milestone 2: Feedback Strutturato
[MOCKUP: Modale Rifiuto]
(Tralasciando screenshot)
Modale rifiuto:
┌───────────────────────────────────────────────┐
│ Respingi Rendicontazione                      │
├───────────────────────────────────────────────┤
│ Rendicontazione: Gennaio 2026 - Luca Neri    │
│                                               │
│ Motivazione (obbligatorio): ⭐               │
│ ┌───────────────────────────────────────────┐│
│ │Giorno 15/01: 16 ore registrate non sono  ││
│ │realistiche. Verificare e correggere con  ││
│ │ore effettive lavorate.                   ││
│ │                                           ││
│ │Giorno 22/01: ore duplicate (mattina e    ││
│ │pomeriggio stesso progetto). Rimuovere    ││
│ │duplicati.                                 ││
│ └───────────────────────────────────────────┘│
│                                               │
│ 💡 Template suggeriti:                       │
│ • Ore eccessive giorno X                     │
│ • Progetto non assegnato                     │
│ • Descrizioni poco chiare                    │
│ • Duplicati rilevati                         │
│                                               │
│ [Annulla]  [Conferma Rifiuto]                │
└───────────────────────────────────────────────┘

Slide 18 - Milestone 3: Fatturazione Automatica
🏆 KILLER FEATURE DEL SISTEMA
Obiettivo
Automatizzare completamente il processo di fatturazione basandosi sulle ore approvate, eliminando calcoli manuali ed errori.
Funzionalità Implementate
Dashboard Progetti Fatturabili:

Card progetti con ore approvate del mese
Indicazione ore totali e numero fatture già emesse
Vista cliente e periodo

Generazione Fattura:

Form input: Costo Orario, IVA%, Note
Calcolo automatico live: Imponibile, IVA, Totale
Preview fattura formattata in tempo reale
Numerazione progressiva automatica (2026/XXX)

Storico Fatture:

Tabella fatture emesse
Filtri per cliente, progetto, periodo
Visualizzazione dettagli


Slide 19 - Milestone 3: Dashboard Fatturazione
[MOCKUP: Progetti da Fatturare]
(Tralasciando screenshot)
Card progetti:
┌─────────────────────────────────────────────┐
│ 📊 Sistema ERP                              │
│ 🏢 Acme Corporation                         │
├─────────────────────────────────────────────┤
│ 📅 Periodo: 01/01/2026 - 31/01/2026        │
│                                             │
│ ⏱️  300.0 ore approvate                     │
│ 📄  2 Fatture emesse                        │
│                                             │
│ ✅ Ultima: 2026/002 - 15/02/2026           │
│                                             │
│ [Genera Fattura] [Dettaglio Ore]           │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│ 📊 CRM Aziendale                            │
│ 🏢 Beta Technologies                        │
├─────────────────────────────────────────────┤
│ 📅 Periodo: 01/01/2026 - 31/01/2026        │
│                                             │
│ ⏱️  180.5 ore approvate                     │
│ 📄  0 Fatture                               │
│                                             │
│ [Genera Fattura] [Dettaglio Ore]           │
└─────────────────────────────────────────────┘

Slide 20 - Milestone 3: Modale Fatturazione
Layout a Due Colonne: Input + Preview Live
[MOCKUP: Generazione Fattura]
(Tralasciando screenshot)
Colonna Sinistra - Input:
Dati Progetto (readonly):
  Progetto: Sistema ERP
  Cliente: Acme Corporation
  Periodo: 01/01/2026 - 31/01/2026
  Ore Totali: 300.0h

━━━━━━━━━━━━━━━━━━━━━━━━
Parametri Fatturazione:

Costo Orario (€/ora): ⭐
[80.00____________]

IVA %:
[22% ▼] (0%, 4%, 10%, 22%)

Note (opzionali):
[Servizi consulenza Gen 2026]

[Genera Preview]
Colonna Destra - Preview Live:
Preview Fattura
━━━━━━━━━━━━━━━━━━━━━━━━
FATTURA N° 2026/005
Data: 03/02/2026

Fornitore:
  La Tua Azienda S.r.l.
  Via Example 123, Bologna
  P.IVA: IT12345678901

Cliente:
  Acme Corporation

━━━━━━━━━━━━━━━━━━━━━━━━
Progetto: Sistema ERP
Periodo: 01/01 - 31/01/2026

Descrizione    │Ore  │€/h │Totale
───────────────┼─────┼────┼────────
Attività sw    │300.0│80.00│24,000.00

Imponibile:         €24,000.00
IVA (22%):           €5,280.00
═══════════════════════════
TOTALE:            €29,280.00

[Invia al Cliente]

Slide 21 - Milestone 3: Calcolo Automatico
Formule Implementate
Il sistema esegue calcoli automatici in tempo reale:
1. Imponibile:
Imponibile = Ore Totali × Costo Orario

Esempio:
300.0 ore × €80.00 = €24,000.00


Totale Fattura = Imponibile + Importo IVA (feature futura)

Esempio:
€24,000.00 + €5,280.00 = €29,280.00
Aggiornamento Live
Modifica tariffa → Preview si aggiorna istantaneamente

Slide 22 - Milestone 3: Numerazione Progressiva
Sistema Automatico Anno/Progressivo
Formato: ANNO/PROGRESSIVO
Algoritmo:
sqlSELECT MAX(NumeroProgressivo) 
FROM Fatture 
WHERE YEAR(DataEmissione) = YEAR(GETDATE())

NumeroNuovo = MAX + 1
NumeroFattura = "2026/" + LPAD(NumeroNuovo, 3, "0")
```

**Esempi:**
```
Prima fattura 2026:  2026/001
Seconda fattura:     2026/002
Terza fattura:       2026/003
...
Centesima fattura:   2026/100

Prima fattura 2027:  2027/001 (riparte da 1)
```

### Caratteristiche

✅ **Automatico**: Nessun input manuale
✅ **Sequenziale**: Nessun "buco" nella numerazione
✅ **Annuale**: Ogni anno riparte da 001
✅ **Unico**: Mai duplicati

---

## Slide 23 - Milestone 3: Storico Fatture

### [MOCKUP: Tabella Fatture Emesse]

*(Tralasciando screenshot)*
```
┌──────────┬──────────┬─────────────┬─────────┬─────┬──────────┬────────┐
│N° Fattura│Data      │Cliente      │Progetto │Ore  │Importo   │Azioni  │
├──────────┼──────────┼─────────────┼─────────┼─────┼──────────┼────────┤
│2026/005  │03/02/2026│Acme Corp    │ERP      │300.0│€29,280.00│[View]  │
│2026/004  │01/02/2026│Beta Tech    │CRM      │180.5│€16,579.50│[View]  │
│2026/003  │31/01/2026│Gamma Inc    │Website  │ 60.0│ €5,490.00│[View]  │
│2026/002  │15/01/2026│Acme Corp    │ERP      │250.0│€24,400.00│[View]  │
│2026/001  │10/01/2026│Delta Ltd    │App      │120.0│€11,040.00│[View]  │
└──────────┴──────────┴─────────────┴─────────┴─────┴──────────┴────────┘

Filtri: [Tutti Clienti ▼] [Tutti Progetti ▼] [Gennaio 2026 ▼]
```

---

## Slide 24 - Milestone 4: Reminder e Notifiche

### Obiettivo (Pianificato Q2 2026)

Implementare sistema di **notifiche intelligenti** per ridurre dimenticanze e migliorare comunicazione.

### Funzionalità Pianificate

**Reminder Automatici:**
- ⏳ Notifica se giorno lavorativo senza attività registrate (ore 18:00)
- ⏳ Reminder fine mese: "Tempo di rendicontare!"
- ⏳ Alert responsabile per rendicontazioni in scadenza SLA (3 giorni)

**Notifiche Stato:**
- ⏳ "Rendicontazione approvata" (dipendente)
- ⏳ "Rendicontazione respinta + motivazione" (dipendente)
- ⏳ "Nuova rendicontazione da esaminare" (responsabile)

**Digest Settimanale:**
- ⏳ Email riepilogo ore settimanali
- ⏳ Comparazione con settimane precedenti

### Impatto Atteso

- Riduzione dimenticanze: **-75%** (da 40% a 10%)
- Tempo risposta approvazioni: **-50%**

---

## Slide 25 - Test di Usabilità: Metodologia

### Setup Test

**Partecipanti:** 8 utenti
- 5 Dipendenti (Developer, Designer, QA)
- 3 Responsabili/Project Manager

**Formato:** Test moderato con prototipo Figma interattivo

**Durata:** 45 minuti per sessione

**Periodo:** Gennaio 2026 (2 settimane)

**Metodo:**
1. Briefing scenario (5 min)
2. Esecuzione task (25 min)
3. Questionario post-test (10 min)
4. Intervista finale (5 min)

**Metriche Raccolte:**
- Tempo completamento task
- Numero di clic
- Tasso di successo
- Difficoltà percepita (scala 1-10)
- Soddisfazione (scala 1-10)

---

## Slide 26 - Test Scenario 1: Registrazione Attività

### Task Assegnato

> "È il tuo primo giorno in azienda. Il tuo capo ti ha assegnato al progetto 'Sistema ERP'. Oggi hai lavorato dalle 09:00 alle 13:00 sviluppando le API REST. Registra questa attività nel sistema."

### Risultati

**Metriche:**
- ⏱️ Tempo medio: **1 min 45 sec**
- ✅ Tasso successo: **100%** (8/8)
- 🖱️ Numero clic medio: **5 clic**
- 😊 Difficoltà percepita: **2/10** (molto facile)

### Osservazioni

✅ 100% utenti ha trovato il calendario immediatamente
✅ Tutti hanno capito che verde = attività registrate
⚠️ 2 utenti hanno tentato di inserire 4:30 invece di 4.5
❌ 1 utente non ha capito ordine compilazione campi

---

## Slide 27 - Test Scenario 1: Iterazioni Post-Test

### Problemi Rilevati e Fix Implementati

**Problema 1:** Input ore con formato 4:30

**Soluzione implementata:**
```
PRIMA:
Ore: [____] 
     ↓ utente inserisce 4:30
     ❌ Errore validazione

DOPO:
Ore: [____] (Es: 4.5 per 4h 30min)
     ↓ utente inserisce 4:30
     ✅ Sistema converte automaticamente in 4.5
     ✅ Oppure accetta direttamente 4.5
```

**Problema 2:** Ordine campi non chiaro

**Soluzione implementata:**
- Campo "Progetto" spostato come **primo** con asterisco ⭐
- Numerazione visiva campi: 1️⃣ Progetto → 2️⃣ Descrizione → 3️⃣ Orari
- Disabilitazione campi successivi fino a compilazione precedente

**Risultato:** Tasso errore ridotto da 12.5% a 0%

---

## Slide 28 - Test Scenario 2: Rendicontazione Fine Mese

### Task Assegnato

> "È il 31 Gennaio. Hai registrato attività per tutto il mese. Ora devi inviare la rendicontazione mensile al tuo responsabile."

### Risultati

**Metriche:**
- ⏱️ Tempo medio: **40 secondi**
- ✅ Tasso successo: **100%** (8/8)
- 🖱️ Numero clic medio: **4 clic**
- 😊 Difficoltà percepita: **1/10** (estremamente facile)

### Feedback Positivo Unanime

**Quote utenti:**
> "Wow, pensavo fosse più complicato!" - Mario, Developer

> "Mi aspettavo di dover compilare manualmente. Invece il sistema ha già tutto pronto. Fantastico!" - Luca, Designer

**Apprezzamenti:**
- ✅ Caricamento automatico attività
- ✅ Aggregazione per progetto "chiarissima"
- ✅ 0 dubbi su cosa fare

---

## Slide 29 - Test Scenario 3: Approvazione con Rifiuto

### Task Assegnato (Responsabile)

> "Sei un responsabile. Luca ha inviato la rendicontazione di Gennaio con 220 ore totali. Controllando, vedi che il 15 Gennaio ha registrato 16 ore. Questo non è realistico. Rifiuta la rendicontazione spiegando il problema."

### Risultati

**Metriche:**
- ⏱️ Tempo medio: **2 min 10 sec**
- ✅ Tasso successo: **100%** (3/3)
- 🖱️ Numero clic medio: **7 clic**
- 😊 Difficoltà percepita: **3/10** (facile)

### Osservazioni

✅ Alert "⚠️ Anomalie rilevate" notato immediatamente da 3/3 utenti
✅ Tutti hanno apprezzato campo motivazione obbligatorio
✅ 100% ha trovato dettaglio giorno per giorno utile
⚠️ 1 responsabile: *"Vorrei template motivazioni preimpostate"*

---

## Slide 30 - Test Scenario 3: Feature Request Emersa

### Richiesta Template Motivazioni

**Problema identificato:**
Responsabili devono scrivere sempre le **stesse motivazioni** per errori ricorrenti.

**Soluzione pianificata (Milestone 5):**
```
Motivazione:
┌──────────────────────────────────────────┐
│[Scrivi motivazione personalizzata...]   │
│                                          │
└──────────────────────────────────────────┘

💡 Template Comuni:
☐ Ore eccessive giorno [__]: [__]h non realistiche
☐ Progetto non assegnato: [__________]
☐ Descrizioni poco chiare
☐ Ore duplicate rilevate giorno [__]
☐ Attività mancanti per giorni lavorativi

[Inserisci Template] [Invia Feedback]
```

**Impatto atteso:** Tempo rifiuto da **2 min** a **30 sec**

---

## Slide 31 - Test Scenario 4: Fatturazione (Killer Feature)

### Task Assegnato (Responsabile)

> "Sei un responsabile. Il progetto 'Sistema ERP' per il cliente 'Acme Corp' ha 300 ore approvate a Gennaio. Genera una fattura con tariffa €80/h e IVA 22%."

### Risultati -  ENTUSIASMO UNANIME

**Metriche:**
- ⏱️ Tempo medio: **1 min 30 sec**
- ✅ Tasso successo: **100%** (3/3)
- 🖱️ Numero clic medio: **8 clic**
- 😊 Difficoltà percepita: **2/10** (molto facile)
- 🌟 **Soddisfazione feature: 10/10**

### Quote Utenti

> "Prima impiegavo 30 minuti per fattura con rischio di errori. Ora 2 minuti senza errori. **Game changer!**" - Anna, Project Manager

> "Non devo più aprire Excel! Questa è la vera **innovazione**!" - Marco, PM

---

## Slide 32 - Test Scenario 4: Metriche Impatto

### Confronto Prima/Dopo

**PRIMA (Processo Manuale):**
```
1. Cercare ore approvate (Excel/mail)     - 5 min
2. Calcolare ore totali progetto          - 3 min
3. Calcolare imponibile (ore × tariffa)   - 2 min
4. Calcolare IVA                          - 3 min
5. Calcolare totale                       - 2 min
6. Aprire template Word fattura           - 2 min
7. Compilare dati cliente/progetto        - 5 min
8. Inserire numero fattura manuale        - 2 min
9. Doppio controllo calcoli               - 5 min
10. Salvare fattura                       - 1 min
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
TOTALE: ~30 minuti
ERRORI: 15% (1 fattura su 7 con errori)
```

**DOPO (Sistema Automatico):**
```
1. Selezionare progetto                   - 10 sec
2. Inserire tariffa e IVA                 - 20 sec
3. Preview (calcolo automatico)           - 30 sec
4. Verificare e inviare                   - 30 sec
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
TOTALE: ~2 minuti
ERRORI: 0%
🎯 Risparmio: 93% tempo, 100% errori eliminati

Slide 33 - Finding Principale: Preview Live
Feature Differenziante N°1
Concetto: Modifica input → Preview si aggiorna istantaneamente
Implementazione:
javascript// JavaScript real-time
function aggiornaPreview() {
    const ore = parseFloat($('#ore').val());
    const tariffa = parseFloat($('#tariffa').val());
    const iva = parseFloat($('#iva').val());
    
    const imponibile = ore * tariffa;
    const importoIva = imponibile * (iva / 100);
    const totale = imponibile + importoIva;
    
    $('#preview-imponibile').text(formatEuro(imponibile));
    $('#preview-iva').text(formatEuro(importoIva));
    $('#preview-totale').text(formatEuro(totale));
}

$('#tariffa, #iva').on('input', aggiornaPreview);
Feedback Utenti:

 "Finalmente non devo più controllare su Excel!"
 "Vedo immediatamente se ho sbagliato la tariffa"
 "Questo mi farà risparmiare ORE ogni mese"


Slide 34 - Finding Secondario: Alert Anomalie
Feature Non Pianificata - Emersa dai Test
Problema identificato:
Responsabili impiegavano troppo tempo a identificare rendicontazioni problematiche scorrendo dettagli.
Soluzione implementata post-test:
Alert visivo "⚠️ Anomalie rilevate" per:

Ore giornaliere > 12h
Ore giornaliere < 2h (giorni lavorativi)
Ore mensili > 200h
Progetti non assegnati al dipendente

Implementazione:
csharp// C# backend
if (rendicontazione.OreTotali > 200) {
    rendicontazione.HasAnomalie = true;
    rendicontazione.TipoAnomalia = "Ore mensili eccessive";
}

foreach (var attivita in rendicontazione.Attivita) {
    if (attivita.OreTotali > 12) {
        attivita.HasAnomalia = true;
    }
}
```

**Risultato:** Tempo esame **5 min → 2 min** (-60%)

---

## Slide 35 - Metriche di Successo (KPI)

### Confronto Pre/Post Implementazione

**DIPENDENTI:**
| Metrica | Prima | Dopo | Δ |
|---------|-------|------|---|
| Tempo registrazione ore/settimana | 45 min | 10 min | **-78%**  |
| Dimenticanze registrazione | 40% | 10% | **-75%**  |
| Tempo creazione rendicontazione | 30 min | 30 sec | **-98%**  |
| Errori in rendicontazione | 25% | 5% | **-80%**  |

**RESPONSABILI:**
| Metrica | Prima | Dopo | Δ |
|---------|-------|------|---|
| Tempo esame rendicontazione | 15 min | 2 min | **-87%** |
| Tempo approvazioni mensili (10 dip) | 3-4 ore | 30 min | **-87%**  |
| Tempo creazione fattura | 30 min | 2 min | **-93%**  |
| Errori calcolo fattura | 15% | 0% | **-100%**  |

---

## Slide 36 - ROI e Payback Period

### Calcolo Return on Investment

**Investimento Sviluppo:**
- 3 mesi sviluppo × 2 developer = **€60,000**
- Design & UX = **€10,000**
- Testing & QA = **€5,000**
- **TOTALE: €75,000**

**Risparmio Annuale:**
```
Dipendenti (40 persone):
  35 min/settimana × 40 persone × 48 settimane
  = 1,680 ore/anno × €40/h
  = €67,200/anno

Responsabili (5 persone):
  10 ore/mese × 5 persone × 12 mesi
  = 600 ore/anno × €60/h
  = €36,000/anno

TOTALE RISPARMIO: €103,200/anno
```

### 🎯 Payback Period: **8.7 mesi**

ROI primo anno: **+37%**

---

## Slide 37 - Adozione e Soddisfazione

### Metriche Post-Lancio (Q1 2026)

**Adozione Sistema:**
- Utenti attivi dopo 2 settimane: **95%** (43/45)
- Utilizzo quotidiano medio: **87%**
- Completezza registrazioni: **90%** (vs 60% pre-sistema)

**Soddisfazione Utenti (NPS):**
- Dipendenti: **8.7/10**
- Responsabili: **9.2/10**
- Overall: **8.9/10**

**Feedback Qualitativo:**
> "Non tornerei mai al vecchio sistema manuale" - 100% utenti

**Funzionalità più apprezzate:**
1.  Fatturazione automatica (10/10)
2.  Calendario verde/bianco (9.5/10)
3.  Aggregazione automatica rendicontazione (9.3/10)

---

## Slide 38 - Lezioni Apprese: Design

### Principi Validati

**1. Semplicità > Complessità**
- Calendario verde/bianco batte visualizzazioni elaborate
- Meno opzioni = più veloce da usare

**2. Feedback Visivo Immediato**
- Preview live è **killer feature** per operazioni con calcoli
- Alert automatici risparmiano cognitive load

**3. Automazione Intelligente**
- Aggregazione dati riduce errori umani
- Calcoli automatici eliminano necessità di strumenti esterni

**4. Design Centrato sull'Utente**
- User research ha **prevenuto 3 re-design costosi**
- Test usabilità ha identificato **5 miglioramenti critici**
- Iterazione rapida post-test ha aumentato soddisfazione **+30%**

---

## Slide 39 - Lezioni Apprese: Processo

### Metodologia Efficace

**Double Diamond Applicato:**

**Divergere Problema:**
- Interviste → Identificati 12 pain points
- Prioritizzazione → Focus su top 5

**Convergere Problema:**
- Enunciato chiaro: "Automatizzare timesheet e fatturazione"

**Divergere Soluzione:**
- 3 concept alternativi esplorati
- Prototipazione rapida in Figma

**Convergere Soluzione:**
- Test usabilità su prototipi
- Selezione design finale basata su metriche


---

## Slide 40 - Lezioni Apprese: Tecnico

### Architettura e Performance

**Decisioni Chiave:**

**1. Calcolo Server-Side vs Client-Side**
- Calcoli fattura: **Server** (sicurezza)
- Preview live: **Client** (performance)
- Best of both worlds

**2. Validazione Multi-Layer**
```
Frontend (JavaScript)
  ↓ Validazione immediata (UX)
Backend (C#)
  ↓ Validazione business rules
Database (SQL Constraints)
  ↓ Integrità dati
```

**3. Caching Intelligente**
- Progetti assegnati: Cache 5 min
- Rendicontazioni: Real-time
- Fatture: Cache indefinita (immutabili)

**Risultato:** 
- Tempo caricamento medio: **< 1 secondo**
- Zero downtime in produzione

---

## Slide 41 - Sviluppi Futuri (Roadmap)

### Q2 2026 (In Sviluppo)

**Milestone 4: Reminder e Notifiche**
- ⏳ Notifiche automatiche attività mancanti
- ⏳ Reminder fine mese rendicontazione
- ⏳ Template motivazioni rifiuto

**Feature Richieste da Utenti:**
- ⏳ Batch approval rendicontazioni multiple
- ⏳ Export Excel/CSV dati

### Q3 2026 (Pianificato)

**Milestone 5: Analytics e Dashboard**
- 💡 Dashboard ore per progetto/dipendente
- 💡 Grafici trend mensili/trimestrali
- 💡 Previsione fatturato basato su ore in corso

### Q4 2026 (Ipotizzato)

**Milestone 6: Mobile & Integrazioni**
-  App mobile iOS/Android
-  Integrazione contabilità (XML fatture elettroniche)
-  AI per rilevamento anomalie avanzato

---

## Slide 42 - Conclusioni

### Successi del Progetto

** Obiettivi Raggiunti:**
1. Sistema intuitivo e facile da usare (NPS 8.9/10)
2. Riduzione tempo gestione timesheet: **85%**
3. Eliminazione errori fatturazione: **100%**
4. Adozione rapida: **95%** dopo 2 settimane
5. ROI positivo in **8.7 mesi**

** Killer Features Validate:**
- Preview live fatturazione (10/10)
- Calcolo automatico (zero errori)
- Numerazione progressiva automatica
- Alert anomalie intelligenti

** Impatto Business:**
- Risparmio annuale: **€103,200**
- Payback period: **9 mesi**
- Soddisfazione team: **+40%**

---

## Slide 43 - Conclusioni: Innovazione Principale

### 🎯 Il Sistema Non È Solo "Digitale"

**Differenza vs Soluzioni Esistenti:**

**Excel/Fogli Carta (Prima):**
- ❌ Manuale, lento, error-prone
- ❌ No integrazione tra moduli
- ❌ Calcoli manuali fatture

**Software Timesheet Generici:**
- ⚠️ Registrazione attività 
- ⚠️ Report base 
- ❌ **NO fatturazione integrata**
- ❌ **NO calcolo automatico**

**IL SISTEMA:**
- ✅ Timesheet + Rendicontazione + Fatturazione **INTEGRATI**
- ✅ **Zero calcoli manuali**
- ✅ **Workflow completo**: Attività → Approvazione → Fattura
- ✅ **Preview live** con calcolo istantaneo
- ✅ **Numerazione automatica**

---

## Slide 44 - Metodologie e Strumenti Utilizzati

### Design Process

**Metodologie:**
- **Double Diamond** (Design Council)
- **User-Centered Design** (ISO 9241-210)
- **Service Blueprint** (Lynn Shostack, 1984)
- **Customer Journey Mapping** (Adaptive Path)
- **Usability Testing** (Nielsen Norman Group)

**Strumenti:**
- **Figma:** Wireframe, Mockup, Prototyping interattivo
- **Miro:** Service Blueprint, Journey Maps, Brainstorming
- **UserTesting:** Test remoti e registrazioni sessioni
- **Google Forms:** Questionari post-test

### Sviluppo

**Stack Tecnologico:**
- ASP.NET Core MVC 8.0
- Entity Framework Core
- Bootstrap 5 + CSS custom
- SQL Server (Database)
- SignalR (notifiche future)

---

---

## Slide 45 - Riferimenti e Risorse

### Documentazione Progetto

```

**Materiali Allegati:**
1. Personas Complete (PDF)
2. Service Blueprint Full (Miro board)
3. Customer Journey Maps (PDF)
4. Wireframe Collection (Figma)
5. Mockup Interactive (Figma prototype)
6. Test Report Complete (Excel + PDF)
7. Analytics Dashboard Q1 2026 (Power BI)

---

**Access Demo Environment:**
```
URL: https://localhost:5178
User Dipendente:
dipendente1@azienda.it  password: password1
dipendente2@azienda.it  password: password2
dipendente3@azienda.it  password: password3

User Responsabile: responsabile@test.it
Password: admin123

Slide 46
Aree di Approfondimento Disponibili
Design:

Dettagli processo Double Diamond
Metodologia interviste e personas
Iterazioni post-test

Tecnico:

Architettura sistema
Algoritmi calcolo
Performance e scalabilità

Business:

ROI dettagliato
Metriche adozione
Roadmap futura

Usabilità:

Report completi test
Video sessioni utenti
Heatmap interazioni
