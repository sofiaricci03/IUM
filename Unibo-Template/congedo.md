# ✈️ Congedo - Guida Dipendente

## Panoramica
Il modulo **Congedo** permette di richiedere ferie, permessi e assenze, che devono essere approvate dal Responsabile prima di essere confermate.

---

## Come Accedere
1. Login con le tue credenziali
2. Nella **sidebar**, clicca su **"Congedo"** (icona aereo)
3. Visualizzerai le tue richieste di congedo

**URL diretto**: `http://localhost:5178/Dipendente/Congedo`

---

## Tipi di Congedo

### 🏖️ Ferie
- **Durata**: Solitamente da 1 giorno a più settimane
- **Retribuite**: Sì
- **Approvazione**: Richiesta
- **Preavviso consigliato**: 2-4 settimane

### 🏥 Permesso (Malattia/Motivi Personali)
- **Durata**: Solitamente da poche ore a pochi giorni
- **Retribuite**: Dipende dal tipo
- **Approvazione**: Richiesta
- **Preavviso**: Il prima possibile

### 📚 Formazione
- **Durata**: Variabile (corsi, conferenze)
- **Retribuite**: Sì, se approvate
- **Approvazione**: Richiesta
- **Preavviso consigliato**: 1-2 settimane

---

## Funzionalità Principali

### 1️⃣ Richiedere un Congedo

#### Step 1: Apri Modale Richiesta
1. Clicca su **"Nuova Richiesta"**
2. Si apre il form di richiesta congedo

#### Step 2: Compila i Dati
**Campi obbligatori**:
- **Tipo**: Seleziona (Ferie / Permesso / Formazione)
- **Data Inizio**: Primo giorno di assenza
- **Data Fine**: Ultimo giorno di assenza
- **Motivo**: Descrizione breve (es: "Vacanza estiva")

**Campi opzionali**:
- **Note**: Informazioni aggiuntive per il Responsabile

**Esempio di compilazione**:
```
Tipo: Ferie
Data Inizio: 15/07/2026
Data Fine: 26/07/2026
Motivo: Vacanze estive in famiglia
Note: Sarò disponibile per emergenze via email
```

#### Step 3: Verifica e Invia
1. Controlla che le date siano corrette
2. Clicca su **"Invia Richiesta"**
3. Ricevi conferma di invio

**Giorni calcolati automaticamente**: Il sistema calcola il numero di giorni lavorativi tra inizio e fine.

### 2️⃣ Stati della Richiesta

Ogni richiesta ha uno stato:

| Stato | Icona | Significato | Azioni Disponibili |
|-------|-------|-------------|-------------------|
| **In Attesa** | 🟡 | Inviata, non ancora esaminata | Visualizza, Annulla |
| **Approvata** | 🟢 | Confermata dal Responsabile | Visualizza |
| **Respinta** | 🔴 | Rifiutata (con motivazione) | Visualizza motivo |

### 3️⃣ Visualizzare le Richieste

Nella pagina principale vedi una **tabella** con:
- **Tipo** di congedo
- **Data Inizio** e **Data Fine**
- **Giorni** totali
- **Stato** (badge colorato)
- **Azioni**: Pulsante Visualizza dettagli

**Filtri disponibili**:
- Per **tipo** di congedo
- Per **stato** (In Attesa / Approvata / Respinta)
- Per **periodo** (mese/anno)

### 4️⃣ Annullare una Richiesta

**Solo per richieste "In Attesa"**:
1. Clicca sull'icona **🗑️ Annulla** nella riga della richiesta
2. Conferma l'annullamento
3. La richiesta viene eliminata

**⚠️ Attenzione**: Non puoi annullare richieste già approvate o respinte!

---

## Workflow Completo

### Ciclo di Richiesta Congedo

```
DIPENDENTE
    ↓
Decide di richiedere ferie
    ↓
Clicca "Nuova Richiesta"
    ↓
Compila form:
- Tipo: Ferie
- Date: 15/07 - 26/07
- Motivo: Vacanze
    ↓
Clicca "Invia Richiesta"
    ↓
STATO: In Attesa (🟡)
    ↓
SISTEMA: Notifica il Responsabile
    ↓
RESPONSABILE: Esamina la richiesta
    ↓
┌─────────────────┬─────────────────┐
│   APPROVA       │    RESPINGE     │
└─────────────────┴─────────────────┘
         ↓                  ↓
   STATO: Approvata   STATO: Respinta
   (🟢)               (🔴)
         ↓                  ↓
   FERIE CONFERMATE   LEGGI MOTIVAZIONE
                           ↓
                    CORREGGI E REINVIA
```

---

## Best Practices

### ✅ Cosa Fare

1. **Richiedi con anticipo**
   - Ferie: Almeno 2-4 settimane prima
   - Permessi pianificati: 1 settimana prima
   - Formazione: Non appena conosci le date

2. **Sii specifico nel motivo**
   - ❌ Male: "Ferie"
   - ✅ Bene: "Vacanza estiva in famiglia - Sardegna"

3. **Coordina con il team**
   - Verifica che altri colleghi non siano assenti nello stesso periodo
   - Avvisa i colleghi prima di richiedere

4. **Controlla il saldo ferie**
   - Verifica quante ferie hai disponibili
   - Non richiedere più giorni di quanti ne hai

5. **Comunica emergenze**
   - Per permessi urgenti (malattia), chiama anche telefonicamente
   - Invia la richiesta comunque per formalizzare

### ❌ Cosa NON Fare

1. **Non richiedere il giorno prima**
   - Serve tempo per organizzare il lavoro
   - Rischi rifiuto

2. **Non essere vago**
   - Il Responsabile deve capire il motivo
   - Motivi chiari aiutano l'approvazione

3. **Non richiedere periodi critici senza coordinamento**
   - Evita scadenze importanti
   - Verifica calendario progetti

4. **Non sparire senza richiesta**
   - Anche per 1 giorno serve richiesta formale
   - Assenze non autorizzate sono gravi

---

## Scenari Comuni

### Scenario 1: Vacanza Estiva (2 settimane)
**Situazione**: Vuoi andare in vacanza ad Agosto

**Quando richiedere**: Maggio/Giugno (2-3 mesi prima)

**Come compilare**:
```
Tipo: Ferie
Data Inizio: 01/08/2026
Data Fine: 14/08/2026
Motivo: Vacanza estiva programmata
Note: Disponibile via email per urgenze
```

**Timeline**:
1. Maggio: Invia richiesta
2. Entro 1 settimana: Ricevi approvazione
3. Luglio: Organizza il passaggio di consegne
4. 01/08: Inizio ferie

### Scenario 2: Permesso per Visita Medica
**Situazione**: Hai una visita medica il pomeriggio del 15 Marzo

**Quando richiedere**: Appena conosci la data (es: 1 settimana prima)

**Come compilare**:
```
Tipo: Permesso
Data Inizio: 15/03/2026 (pomeriggio)
Data Fine: 15/03/2026
Motivo: Visita medica specialistica
Note: Assenza dalle 14:00 alle 18:00
```

**Timeline**:
1. 8 Marzo: Invia richiesta
2. Entro 2 giorni: Ricevi approvazione
3. 15 Marzo: Vai alla visita

### Scenario 3: Formazione (Conferenza)
**Situazione**: Vuoi partecipare a una conferenza di 3 giorni

**Quando richiedere**: 2-3 settimane prima

**Come compilare**:
```
Tipo: Formazione
Data Inizio: 20/05/2026
Data Fine: 22/05/2026
Motivo: Conferenza "Web Development 2026" - Milano
Note: Corso approvato da budget formazione
```

**Timeline**:
1. 1 Maggio: Invia richiesta
2. Entro 1 settimana: Ricevi approvazione
3. 20-22 Maggio: Partecipa alla conferenza

### Scenario 4: Malattia Improvvisa
**Situazione**: Ti svegli con la febbre alta

**Azione immediata**:
1. **Telefona** al Responsabile (08:00-09:00)
2. Invia richiesta congedo dal telefono/PC:
```
Tipo: Permesso
Data Inizio: 10/02/2026 (oggi)
Data Fine: 10/02/2026 (o più giorni se necessario)
Motivo: Malattia - Febbre alta
Note: Certificato medico seguirà
```
3. Vai dal medico e ottieni certificato
4. Invia certificato all'HR

---

## Gestione Saldo Ferie

### Visualizzare il Saldo
**Dove trovarlo**: Solitamente nella pagina "Congedo" o "Profilo"

**Informazioni tipiche**:
```
Ferie Totali Annuali: 22 giorni
Ferie Utilizzate: 8 giorni
Ferie Rimanenti: 14 giorni

Permessi Annuali: 104 ore (equivalenti a 13 giorni)
Permessi Utilizzati: 16 ore (2 giorni)
Permessi Rimanenti: 88 ore (11 giorni)
```

### Pianificazione Ferie

**Strategia consigliata**:
1. **Pianifica l'anno**: Distribuisci le 22 giornate nell'anno
2. **Riserva per imprevisti**: Tieni 2-3 giorni di buffer
3. **Coordina con colleghi**: Evita sovrapposizioni
4. **Monitora il saldo**: Controlla regolarmente quante ferie hai

**Esempio di pianificazione annuale**:
```
📅 Gennaio: 0 giorni
📅 Febbraio: 0 giorni
📅 Marzo: 3 giorni (lungo weekend)
📅 Aprile: 0 giorni
📅 Maggio: 2 giorni (ponti)
📅 Giugno: 0 giorni
📅 Luglio: 0 giorni
📅 Agosto: 10 giorni (vacanza principale)
📅 Settembre: 0 giorni
📅 Ottobre: 2 giorni (ponte)
📅 Novembre: 0 giorni
📅 Dicembre: 5 giorni (festività)

TOTALE: 22 giorni ✅
```

---

## Calcolo Giorni

### Giorni Lavorativi vs Giorni Solari

**Il sistema calcola solo i giorni lavorativi** (Lunedì-Venerdì)

**Esempio**:
```
Data Inizio: Lunedì 15/07
Data Fine: Venerdì 26/07

Calendario:
Lun Mar Mer Gio Ven | Sab Dom  (Settimana 1)
 15  16  17  18  19  | 20  21   → 5 giorni

Lun Mar Mer Gio Ven | Sab Dom  (Settimana 2)
 22  23  24  25  26  | 27  28   → 5 giorni

TOTALE GIORNI LAVORATIVI: 10 giorni
TOTALE GIORNI SOLARI: 12 giorni
```

**Giorni addebitati**: Solo i 10 giorni lavorativi!

---

## FAQ

**Q: Quante ferie ho all'anno?**  
A: Solitamente **22 giorni lavorativi** (può variare in base al CCNL e contratto).

**Q: Le ferie si possono accumulare?**  
A: Dipende dalla policy aziendale. Solitamente vanno utilizzate entro l'anno, con possibile riporto parziale all'anno successivo.

**Q: Cosa succede se non uso tutte le ferie?**  
A: Rischi di **perderle** se non consumate entro i termini. Pianifica bene!

**Q: Posso richiedere ferie durante il periodo di prova?**  
A: Dipende dal contratto. Solitamente **sì**, ma con alcune limitazioni.

**Q: Posso annullare ferie già approvate?**  
A: **Dipende**. Contatta il Responsabile e spiega la situazione. Potrebbe richiedere una nuova richiesta.

**Q: Quanto tempo ho per ricevere risposta?**  
A: Solitamente **3-5 giorni lavorativi**. Per urgenze, contatta direttamente il Responsabile.

**Q: Cosa fare se la richiesta viene respinta?**  
A: Leggi la motivazione, contatta il Responsabile per chiarimenti, valuta alternative (es: altri periodi).

**Q: I permessi sono retribuiti?**  
A: Dipende dal tipo. **Malattia**: sì (con certificato). **Motivi personali**: dipende dal contratto e dalla policy.

---

## Risoluzione Problemi

### Problema: Non riesco a inviare la richiesta
**Causa**: Date errate o campi obbligatori mancanti  
**Soluzione**:
1. Verifica che Data Fine ≥ Data Inizio
2. Compila tutti i campi obbligatori
3. Controlla che il motivo sia presente

### Problema: La richiesta è "bloccata" in In Attesa da settimane
**Causa**: Il Responsabile non ha ancora risposto  
**Soluzione**:
1. Invia un promemoria via email
2. Contatta direttamente il Responsabile
3. Escalate all'HR se necessario

### Problema: Ho sbagliato le date
**Situazione 1 - Richiesta ancora In Attesa**:
- Annulla la richiesta
- Crea una nuova richiesta con le date corrette

**Situazione 2 - Richiesta già Approvata**:
- Contatta il Responsabile
- Spiega l'errore
- Richiedi modifica o annullamento

---

## Checklist Prima di Richiedere

Prima di inviare, verifica:

- [ ] Ho controllato il saldo ferie/permessi
- [ ] Le date sono corrette
- [ ] Il motivo è chiaro e specifico
- [ ] Ho avvisato i colleghi
- [ ] Non ci sono scadenze critiche in quel periodo
- [ ] Ho verificato che altri colleghi non siano già assenti
- [ ] Sto richiedendo con adeguato preavviso

✅ **Se tutto è verificato, invia la richiesta!**

---

## Supporto
Per assistenza:
- 📧 Email: hr@azienda.it
- 📞 Telefono: +39 051 1234567
- 💬 Chat: #supporto-hr

---

**Ultima modifica**: Febbraio 2026  
**Versione**: 1.0
