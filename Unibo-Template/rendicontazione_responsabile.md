# ✅ Rendicontazione - Guida Responsabile

## Panoramica
Il modulo **Rendicontazione** permette di visualizzare, esaminare e approvare/respingere le rendicontazioni mensili inviate dai dipendenti.

---

## Come Accedere
1. Login con credenziali Responsabile
2. Nella **sidebar**, clicca su **"Rendicontazione"**
3. Visualizzerai tutte le rendicontazioni dei tuoi dipendenti

**URL diretto**: `http://localhost:5178/Responsabile/Rendicontazione`

---

## Funzionalità Principali

### 1️⃣ Visualizzare Rendicontazioni

Vedi una **tabella** con:
- **Dipendente**: Nome e cognome
- **Mese/Anno**: Periodo rendicontato
- **Ore Totali**: Somma ore del mese
- **Data Invio**: Quando è stata inviata
- **Stato**: 
  - 🟡 **In Attesa**: Da esaminare
  - 🟢 **Approvata**: Confermata
  - 🔴 **Respinta**: Rifiutata
- **Azioni**: Esamina, Approva, Respingi

### 2️⃣ Esaminare una Rendicontazione

#### Come Aprire
1. Clicca su **"Esamina"** (icona 👁️) nella riga
2. Si apre il **dettaglio completo**

#### Informazioni Visualizzate

**Intestazione**:
```
Rendicontazione: Gennaio 2026
Dipendente: Mario Verdi
Data Invio: 05/02/2026
Stato: In Attesa
```

**Breakdown per Progetto**:
```
📁 Sistema ERP (Cliente: Acme)
   - 05/01/2026: Sviluppo API REST (4.0h)
   - 06/01/2026: Testing moduli (3.5h)
   - 10/01/2026: Documentazione (2.5h)
   ...
   Totale Progetto: 60.0 ore

📁 CRM Aziendale (Cliente: Beta Corp)
   - 07/01/2026: Design UI (6.0h)
   - 08/01/2026: Implementazione form (5.0h)
   ...
   Totale Progetto: 45.0 ore

🎯 ORE TOTALI MESE: 105.0 ore
```

**Note Dipendente** (se presenti):
```
Note: Alcune ore extra per scadenza progetto ERP del 15/01
```

### 3️⃣ Approvare una Rendicontazione

#### Quando Approvare?
✅ Le ore sono **realistiche** e verificabili  
✅ I progetti sono **corretti**  
✅ Le descrizioni sono **chiare**  
✅ Non ci sono **duplicati** o errori evidenti  
✅ Le ore corrispondono alle **aspettative**  

#### Come Approvare
1. Esamina la rendicontazione
2. Verifica tutti i dettagli
3. Clicca su **"Approva Rendicontazione"**
4. Conferma l'approvazione

**Effetto**:
- Stato diventa: 🟢 **Approvata**
- Le ore diventano **fatturabili**
- Il dipendente riceve **notifica** (se configurata)

### 4️⃣ Respingere una Rendicontazione

#### Quando Respingere?
❌ Ore **non veritiere** o esagerate  
❌ Attività su **progetti sbagliati**  
❌ Descrizioni **poco chiare** o mancanti  
❌ **Duplicati** evidenti  
❌ Errori di calcolo o **incoerenze**  

#### Come Respingere
1. Esamina la rendicontazione
2. Identifica il problema
3. Clicca su **"Respingi Rendicontazione"**
4. **OBBLIGATORIO**: Inserisci **motivazione** chiara
5. Conferma il rifiuto

**Esempio motivazioni**:
```
✅ Buona: "Progetto ERP: 80 ore registrate ma scadenza 
          era per 40 ore. Verifica e correggi."
          
✅ Buona: "Attività del 15/01 duplicate (mattina/pomeriggio). 
          Rimuovi duplicati e reinvia."

❌ Male: "Errore" (troppo vago)
❌ Male: "No" (non spiega il problema)
```

**Effetto**:
- Stato diventa: 🔴 **Respinta**
- Il dipendente **vede la motivazione**
- Deve **correggere** e **reinviare**

---

## Workflow Completo

### Ciclo Approvazione Standard

```
FINE MESE
    ↓
DIPENDENTE: Invia rendicontazione
    ↓
NOTIFICA RESPONSABILE
    ↓
RESPONSABILE: Vede rendicontazione "In Attesa"
    ↓
RESPONSABILE: Clicca "Esamina"
    ↓
RESPONSABILE: Controlla:
- Ore totali ragionevoli?
- Progetti corretti?
- Descrizioni chiare?
- Tutto coerente?
    ↓
┌─────────────────┬─────────────────┐
│   SÌ, OK        │    NO, ERRORI   │
│   ↓             │    ↓            │
│   APPROVA       │    RESPINGI     │
│   ↓             │    ↓            │
│   🟢 Approvata  │    🔴 Respinta  │
│   ↓             │    ↓            │
│   ORE           │    DIPENDENTE   │
│   FATTURABILI   │    CORREGGE     │
│                 │    ↓            │
│                 │    REINVIA      │
│                 │    ↓            │
│                 │    (torna sopra)│
└─────────────────┴─────────────────┘
```

---

## Scenari Comuni

### Scenario 1: Rendicontazione Perfetta

**Situazione**: Mario ha inviato rendicontazione Gennaio

**Esame**:
```
Dipendente: Mario Verdi
Mese: Gennaio 2026
Ore Totali: 160 ore (20 giorni × 8h)

Progetti:
- ERP Development: 120 ore
- Support: 40 ore

Descrizioni: ✅ Chiare e dettagliate
Duplicati: ❌ Nessuno
Coerenza: ✅ Tutto ok
```

**Azione**:
1. Clicca "Esamina"
2. Leggi breakdown
3. Tutto corretto
4. Clicca "Approva"
5. ✅ Mario notificato → Ore fatturabili

### Scenario 2: Ore Esagerate

**Situazione**: Luca ha registrato troppe ore

**Esame**:
```
Dipendente: Luca Neri
Mese: Gennaio 2026
Ore Totali: 220 ore (!)

Problema: 220 ore / 20 giorni = 11 ore/giorno
          Sembra eccessivo, da verificare
```

**Azione**:
1. Clicca "Esamina"
2. Controllo dettagliato:
   - Giorno 15/01: 14 ore registrate (!)
   - Giorno 22/01: 15 ore registrate (!)
3. Non credibile
4. Clicca "Respingi"
5. Motivazione:
   ```
   Ore mensili eccessive (220 ore). 
   Giorni 15/01 e 22/01 con 14-15 ore non realistici.
   Verifica e correggi con ore effettive.
   ```
6. Invia rifiuto
7. 🔴 Luca deve correggere e reinviare

### Scenario 3: Progetti Sbagliati

**Situazione**: Anna ha registrato ore su progetto non assegnato

**Esame**:
```
Dipendente: Anna Bianchi
Mese: Gennaio 2026

Progetti:
- CRM Development: 80 ore ✅ (assegnata)
- Sistema ERP: 60 ore ❌ (NON assegnata!)

Problema: Anna non è assegnata a "Sistema ERP"
```

**Azione**:
1. Clicca "Respingi"
2. Motivazione:
   ```
   60 ore registrate su "Sistema ERP" ma non sei 
   assegnata a quel progetto. Verifica e correggi 
   con il progetto corretto o chiedi assegnazione.
   ```
3. Invia rifiuto

### Scenario 4: Multipli Dipendenti - Fine Mese

**Situazione**: 31 Gennaio, 10 dipendenti inviano rendicontazioni

**Gestione**:
```
PRIORITÀ ALTA (scadenza imminente):
1. ✅ Mario Verdi - 160h → Approva subito
2. ✅ Luca Neri - 155h → Approva subito

VERIFICA NECESSARIA:
3. 🔍 Anna Bianchi - 90h → Esamina attentamente
4. 🔍 Paolo Rossi - 200h → Ore sospette, respingi

STANDARD:
5-10. Esamina e approva/respingi nei prossimi 2 giorni
```

**Strategia**:
- Approva prima quelle urgenti e corrette
- Dedica tempo extra a quelle sospette
- Respingi rapidamente se errori evidenti
- Approva tutte entro 3-5 giorni lavorativi

---

## Best Practices

### ✅ Cosa Fare

1. **Esamina entro 3-5 giorni**
   - Non far aspettare troppo i dipendenti
   - Le ore diventano fatturabili solo dopo approvazione

2. **Controlla sempre i dettagli**
   - Non approvare "a scatola chiusa"
   - Verifica breakdown per progetto
   - Controlla ore/giorno ragionevoli

3. **Motivazioni chiare quando respingi**
   - Spiega esattamente cosa è sbagliato
   - Indica come correggere
   - Sii costruttivo, non punitivo

4. **Comunica preventivamente**
   - Se sospetti problemi, parla col dipendente PRIMA
   - "Mario, ho visto 200 ore, è corretto?"
   - Evita sorprese

5. **Traccia pattern**
   - Dipendente che sbaglia spesso? Training needed
   - Dipendente sempre perfetto? Riconoscilo

### ❌ Cosa NON Fare

1. **Non approvare senza esaminare**
   - Rischi di fatturare ore non reali
   - Problemi con cliente

2. **Non respingere senza motivazione**
   - Il dipendente non capisce l'errore
   - Frustrazione e rifiuti ripetuti

3. **Non aspettare settimane**
   - I dipendenti restano "bloccati"
   - Cash flow aziendale ne risente

4. **Non essere troppo severo**
   - Piccoli errori sono normali
   - Valuta caso per caso

5. **Non ignorare pattern negativi**
   - Dipendente che sbaglia sempre? Intervieni
   - Non approvare solo per evitare conflitti

---

## Filtri e Ricerca

### Filtrare Rendicontazioni

**Per Stato**:
- Solo "In Attesa" → Quelle da esaminare
- Solo "Approvate" → Archivio approvate
- Solo "Respinte" → Quelle rifiutate

**Per Dipendente**:
- Seleziona dipendente specifico
- Vedi storico completo

**Per Periodo**:
- Filtra per mese/anno
- Utile per report

**Ordinamento**:
- Per data invio (più recenti prime)
- Per dipendente (alfabetico)
- Per ore (maggiori prime)

---

## Reportistica

### Metriche Utili

**Per Mese**:
```
Gennaio 2026
─────────────────────────────
Rendicontazioni Ricevute: 10
Approvate: 7
Respinte: 2
In Attesa: 1

Ore Totali Approvate: 1,250h
Ore Medie per Dipendente: 178.5h
```

**Per Dipendente**:
```
Mario Verdi
─────────────────────────────
Rendicontazioni 2026: 12
Approvate: 12 (100%)
Respinte: 0
Ore Totali: 1,920h
Media Mensile: 160h
```

---

## Integrazione con Fatturazione

### Flusso Completo

```
RENDICONTAZIONE → FATTURAZIONE

Step 1: Dipendenti registrano ore
Step 2: Dipendenti inviano rendicontazioni
Step 3: RESPONSABILE APPROVA
        ↓
        ✅ ORE DIVENTANO FATTURABILI
        ↓
Step 4: Vai su "Fatturazione"
Step 5: Vedi progetti con ore approvate
Step 6: Genera fatture per clienti
```

**Importante**: Solo le **ore approvate** sono fatturabili!

---

## FAQ

**Q: Devo approvare ogni singola attività?**  
A: No, approvi l'intera rendicontazione mensile.

**Q: Posso approvare parzialmente?**  
A: No, è tutto o niente. Se ci sono errori, respingi con motivazione specifica.

**Q: Cosa succede se non approvo entro un termine?**  
A: Le ore rimangono non fatturabili. Il dipendente non può procedere.

**Q: Posso annullare un'approvazione?**  
A: Dipende dall'implementazione. Solitamente no, per tracciabilità.

**Q: Quanto tempo ho per approvare?**  
A: Best practice: 3-5 giorni lavorativi. Controlla policy aziendale.

**Q: Il dipendente può rivedere la motivazione del rifiuto?**  
A: Sì, vede la motivazione nella sua area.

---

## Checklist Esame Rendicontazione

Prima di approvare, verifica:

- [ ] Ore totali ragionevoli (es: 160h per mese full-time)
- [ ] Ore/giorno credibili (es: 6-10h/giorno)
- [ ] Progetti corretti (dipendente assegnato)
- [ ] Descrizioni chiare e comprensibili
- [ ] Nessun duplicato evidente
- [ ] Date nel range corretto
- [ ] Note dipendente lette (se presenti)
- [ ] Coerenza con progetti in corso

✅ **Se tutto ok, approva. Altrimenti, respingi con motivazione chiara.**

---

## Supporto
Per assistenza:
- 📧 Email: admin@azienda.it
- 📞 Telefono: +39 051 1234567

---

**Ultima modifica**: Febbraio 2026  
**Versione**: 1.0
