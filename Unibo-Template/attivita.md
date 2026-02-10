# 📅 Attività - Guida Dipendente

## Panoramica
Il modulo **Attività** permette ai dipendenti di registrare quotidianamente le ore lavorate su progetti specifici attraverso un calendario interattivo.

---

## Come Accedere
1. Effettua il login con le tue credenziali
2. Nella **sidebar laterale**, clicca su **"Attività"** (icona calendario)
3. Si aprirà il calendario mensile con le tue attività

**URL diretto**: `http://localhost:5178/Dipendente/Dashboard`

---

## Funzionalità Principali

### 1️⃣ Calendario Mensile
- **Vista mensile** con tutti i giorni del mese corrente
- **Navigazione** tra i mesi con frecce ← →
- **Colori distintivi**:
  - 🟢 Verde: Giorni con attività registrate
  - ⚪ Bianco: Giorni senza attività
  - 🔵 Blu (bordo): Giorno odierno

### 2️⃣ Registrazione Attività

#### Creazione Nuova Attività
1. Clicca su un **giorno nel calendario**
2. Si apre il modale "Registra Attività"
3. Compila i campi:
   - **Progetto**: Seleziona dall'elenco dei progetti assegnati
   - **Descrizione**: Cosa hai fatto (es: "Sviluppo API REST")
   - **Ora Inizio**: Orario di inizio (es: 09:00)
   - **Ora Fine**: Orario di fine (es: 13:00)
4. Clicca su **"Salva Attività"**

**Calcolo automatico**: Il sistema calcola automaticamente le ore lavorate (Ora Fine - Ora Inizio)

#### Modifica Attività Esistente
1. Clicca su un **giorno con attività** (verde)
2. Nella lista attività del giorno, clicca sull'icona **✏️ Modifica**
3. Modifica i campi necessari
4. Clicca su **"Salva Attività"**

#### Eliminazione Attività
1. Clicca su un giorno con attività
2. Clicca sull'icona **🗑️ Elimina** sull'attività da rimuovere
3. Conferma l'eliminazione

### 3️⃣ Visualizzazione Dettagli Giornalieri
Cliccando su un giorno, puoi vedere:
- **Data selezionata**
- **Lista completa** di tutte le attività della giornata
- Per ogni attività:
  - Nome progetto
  - Descrizione
  - Orario (es: 09:00 - 13:00)
  - Ore totali (es: 4.0h)
  - Pulsanti Modifica/Elimina

### 4️⃣ Statistiche Mensili
Nel riquadro in alto a destra:
- **Ore Totali Mese**: Somma di tutte le ore lavorate nel mese
- **Giorni Lavorati**: Numero di giorni con almeno 1 attività
- **Media Ore/Giorno**: Ore totali ÷ Giorni lavorati

---

## Workflow Tipico

### Scenario: Registrare una Giornata di Lavoro

**Mattina - Progetto ERP:**
1. Apri il calendario
2. Clicca su "Oggi"
3. Seleziona progetto "Sistema ERP"
4. Descrizione: "Sviluppo modulo fatturazione"
5. Orario: 09:00 - 13:00
6. Salva

**Pomeriggio - Progetto CRM:**
1. Clicca di nuovo su "Oggi"
2. Seleziona progetto "CRM Aziendale"
3. Descrizione: "Testing interfaccia utente"
4. Orario: 14:00 - 18:00
5. Salva

**Risultato**: Hai registrato 8 ore totali (4h + 4h) su 2 progetti diversi.

---

## Regole e Vincoli

### ✅ Permessi
- ✅ Registrare attività solo su **progetti assegnati**
- ✅ Modificare/eliminare **solo le proprie attività**
- ✅ Registrare attività **per qualsiasi data**

### ⚠️ Limitazioni
- ❌ Non puoi registrare attività su progetti **non assegnati**
- ❌ L'orario di fine deve essere **successivo** all'orario di inizio
- ❌ Non puoi creare attività con **ore negative**

### 📝 Best Practices
1. **Registra quotidianamente**: Non aspettare fine settimana/mese
2. **Descrizioni chiare**: Spiega cosa hai fatto (utile per report)
3. **Orari precisi**: Registra gli orari reali di lavoro
4. **Controlla mensile**: Prima di inviare la rendicontazione, verifica le ore totali

---

## Integrazione con Rendicontazione

### Flusso Attività → Rendicontazione

```
1. Dipendente registra attività quotidiane nel CALENDARIO
   ↓
2. A fine mese, va su RENDICONTAZIONE
   ↓
3. Vede tutte le attività del mese raggruppate
   ↓
4. Invia rendicontazione al Responsabile
   ↓
5. Responsabile approva/respinge
   ↓
6. Se approvata → Le ore diventano fatturabili
```

**Importante**: Le attività registrate qui verranno automaticamente incluse nella rendicontazione mensile!

---

## FAQ

**Q: Posso registrare attività per date passate?**  
A: Sì, puoi cliccare su qualsiasi giorno del calendario e registrare attività.

**Q: Cosa succede se dimentico di registrare un'attività?**  
A: Nessun problema! Puoi sempre tornare indietro e registrarla prima di inviare la rendicontazione mensile.

**Q: Posso registrare più attività nello stesso giorno?**  
A: Sì! Puoi registrare più attività sullo stesso giorno, anche su progetti diversi.

**Q: Posso modificare un'attività dopo aver inviato la rendicontazione?**  
A: Dipende dallo stato della rendicontazione:
- **Bozza**: Sì, modificabile
- **Inviata/Approvata**: No, contatta il tuo Responsabile

**Q: Come faccio a vedere le ore totali del mese?**  
A: Le statistiche in alto a destra mostrano sempre le ore totali del mese visualizzato.

**Q: I weekend e festivi sono conteggiati?**  
A: Puoi registrare attività anche nei weekend. Il conteggio ore non fa distinzione tra giorni feriali e festivi.

---

## Risoluzione Problemi

### Problema: Non vedo i miei progetti nell'elenco
**Causa**: Non sei stato assegnato a nessun progetto  
**Soluzione**: Contatta il tuo Responsabile per farti assegnare ai progetti

### Problema: Il calendario non carica le attività
**Causa**: Errore di connessione o problema server  
**Soluzione**: 
1. Ricarica la pagina (F5)
2. Controlla la connessione internet
3. Contatta il supporto IT se il problema persiste

### Problema: L'orario di fine è prima dell'orario di inizio
**Causa**: Errore di compilazione  
**Soluzione**: Verifica gli orari inseriti. L'ora di fine deve essere successiva all'ora di inizio.

---

## Glossario

- **Attività**: Una singola registrazione di ore lavorate su un progetto specifico
- **Progetto**: Un'iniziativa aziendale a cui sei stato assegnato
- **Rendicontazione**: Il report mensile che raggruppa tutte le tue attività
- **Ore fatturabili**: Ore approvate dal Responsabile che possono essere fatturate al cliente

---

