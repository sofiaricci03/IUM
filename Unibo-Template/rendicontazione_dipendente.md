# 📊 Rendicontazione - Guida Dipendente

## Panoramica
Il modulo **Rendicontazione** permette di inviare mensilmente al Responsabile il riepilogo delle ore lavorate, che devono essere approvate prima di diventare fatturabili.

---

## Come Accedere
1. Login con le tue credenziali
2. Nella **sidebar**, clicca su **"Rendicontazione"** (icona clipboard)
3. Visualizzerai le tue rendicontazioni mensili

**URL diretto**: `http://localhost:5178/Dipendente/Rendicontazione`

---

## Funzionalità Principali

### 1️⃣ Vista Rendicontazioni Mensili

Nella pagina principale vedi:
- **Lista delle rendicontazioni** per mese/anno
- **Stato** di ogni rendicontazione:
  - 🟡 **Bozza**: Non ancora inviata, modificabile
  - 🔵 **Inviata**: In attesa di approvazione
  - 🟢 **Approvata**: Confermata dal Responsabile
  - 🔴 **Respinta**: Rifiutata (con motivazione)

### 2️⃣ Creazione Rendicontazione Mensile

#### Step 1: Seleziona Mese e Anno
1. Clicca su **"Nuova Rendicontazione"**
2. Seleziona:
   - **Mese** (es: Gennaio)
   - **Anno** (es: 2026)
3. Clicca su **"Genera Rendicontazione"**

#### Step 2: Verifica Attività
Il sistema:
- Carica **automaticamente** tutte le attività del mese dal calendario
- Raggruppa le attività **per progetto**
- Calcola le **ore totali**

**Esempio di rendicontazione generata:**

```
📁 Sistema ERP
   - 05/01/2026: Sviluppo API (4.0h)
   - 06/01/2026: Testing moduli (3.5h)
   - 10/01/2026: Documentazione (2.0h)
   Totale: 9.5 ore

📁 CRM Aziendale
   - 07/01/2026: Design UI (6.0h)
   - 08/01/2026: Integrazione DB (5.0h)
   Totale: 11.0 ore

🎯 ORE TOTALI MESE: 20.5 ore
```

#### Step 3: Aggiungi Note (Opzionale)
- Campo **Note**: Aggiungi commenti o informazioni utili per il Responsabile
- Esempio: "Alcune ore extra per scadenza del 15/01"

#### Step 4: Invia Rendicontazione
1. Verifica che tutto sia corretto
2. Clicca su **"Invia Rendicontazione"**
3. Conferma l'invio

**⚠️ Attenzione**: Dopo l'invio, **non potrai più modificare** la rendicontazione!

---

## Stati della Rendicontazione

### 🟡 Bozza
**Cosa significa**: Rendicontazione creata ma non ancora inviata

**Azioni disponibili**:
- ✏️ Modifica note
- 📤 Invia al Responsabile
- 🗑️ Elimina

**Quando usare**: Quando vuoi preparare la rendicontazione ma non sei pronto per inviarla

### 🔵 Inviata
**Cosa significa**: In attesa di approvazione dal Responsabile

**Azioni disponibili**:
- 👁️ Visualizza dettagli
- ⏳ Attendi approvazione

**Tempistiche**: Solitamente il Responsabile risponde entro 3-5 giorni lavorativi

### 🟢 Approvata
**Cosa significa**: Confermata dal Responsabile, ore fatturabili

**Azioni disponibili**:
- 👁️ Visualizza
- 📥 Scarica PDF (se disponibile)

**Effetto**: Le ore diventano **fatturabili** e incluse nei calcoli aziendali

### 🔴 Respinta
**Cosa significa**: Rifiutata dal Responsabile (con motivazione)

**Azioni disponibili**:
- 👁️ Visualizza motivo rifiuto
- ✏️ Correggi e reinvia

**Cosa fare**:
1. Leggi la motivazione del Responsabile
2. Correggi le attività nel calendario
3. Crea una nuova rendicontazione
4. Reinvia

---

## Workflow Completo

### Ciclo Mensile Standard

```
INIZIO MESE
    ↓
DIPENDENTE: Registra attività quotidiane nel calendario
    ↓
FINE MESE (es: 31 Gennaio)
    ↓
DIPENDENTE: Va su "Rendicontazione"
    ↓
DIPENDENTE: Clicca "Nuova Rendicontazione"
    ↓
SISTEMA: Genera automaticamente il riepilogo
    ↓
DIPENDENTE: Verifica ore e progetti
    ↓
DIPENDENTE: Aggiunge eventuali note
    ↓
DIPENDENTE: Clicca "Invia Rendicontazione"
    ↓
STATO: Inviata (🔵)
    ↓
RESPONSABILE: Riceve notifica
    ↓
RESPONSABILE: Esamina e decide
    ↓
┌─────────────────┬─────────────────┐
│   APPROVA       │    RESPINGE     │
└─────────────────┴─────────────────┘
         ↓                  ↓
   STATO: Approvata   STATO: Respinta
   (🟢)               (🔴)
         ↓                  ↓
   ORE FATTURABILI    CORREGGI & REINVIA
```

---

## Best Practices

### ✅ Cosa Fare

1. **Invia entro il 5 del mese successivo**
   - Esempio: Rendicontazione Gennaio → Invia entro 5 Febbraio
   
2. **Controlla prima di inviare**
   - Verifica che tutte le ore siano registrate
   - Controlla i totali per progetto
   - Assicurati che le descrizioni siano chiare

3. **Usa le note per comunicare**
   - Spiega eventuali anomalie
   - Segnala ore straordinarie
   - Indica progetti particolari

4. **Tieni traccia**
   - Salva una copia delle rendicontazioni approvate
   - Confronta mese su mese

### ❌ Cosa NON Fare

1. **Non aspettare l'ultimo momento**
   - Rischi di dimenticare attività
   - Meno tempo per correzioni

2. **Non inviare senza verificare**
   - Le rendicontazioni inviate non sono modificabili
   - Errori causano rifiuti e ritardi

3. **Non registrare ore false**
   - Registra solo ore effettivamente lavorate
   - Sii onesto e preciso

---

## Scenari Comuni

### Scenario 1: Primo Mese di Lavoro
**Situazione**: È il tuo primo mese, non sai cosa fare

**Soluzione**:
1. Durante il mese: Registra attività quotidiane nel calendario
2. Fine mese: Vai su Rendicontazione
3. Clicca "Nuova Rendicontazione"
4. Seleziona il mese corrente
5. Verifica che le attività siano tutte presenti
6. Invia

### Scenario 2: Rendicontazione Respinta
**Situazione**: Il Responsabile ha respinto la tua rendicontazione

**Soluzione**:
1. Apri la rendicontazione respinta
2. Leggi la **motivazione** del Responsabile
3. Vai su "Attività" (calendario)
4. Correggi le attività come richiesto
5. Torna su "Rendicontazione"
6. Crea una nuova rendicontazione per lo stesso mese
7. Reinvia

**Motivazioni comuni di rifiuto**:
- Ore non veritiere
- Attività su progetti sbagliati
- Descrizioni poco chiare
- Ore duplicate

### Scenario 3: Dimenticato di Registrare Attività
**Situazione**: Hai dimenticato di registrare alcune giornate

**Soluzione**:
1. **Prima di inviare**: Vai sul calendario, registra le attività mancanti, poi crea la rendicontazione
2. **Dopo aver inviato**: Contatta il Responsabile e spiega la situazione

### Scenario 4: Lavoro su Più Progetti
**Situazione**: Hai lavorato su 5 progetti diversi nel mese

**Soluzione**:
- La rendicontazione **raggruppa automaticamente** per progetto
- Verifica che ogni progetto abbia le ore corrette
- Aggiungi note se necessario per spiegare la distribuzione

---

## Calcoli e Statistiche

### Ore Totali Mese
```
Ore Totali = Σ (Ora Fine - Ora Inizio) per tutte le attività del mese
```

**Esempio**:
- 20 giorni lavorati
- 8 ore al giorno in media
- **Ore Totali = 160 ore**

### Ore per Progetto
```
Ore Progetto X = Σ ore di attività sul Progetto X
```

**Esempio**:
- Sistema ERP: 80 ore
- CRM: 60 ore
- Sito Web: 20 ore
- **Totale: 160 ore**

---

## Domande Frequenti (FAQ)

**Q: Quando devo inviare la rendicontazione?**  
A: Entro il **5 del mese successivo**. Esempio: Rendicontazione Gennaio → Entro 5 Febbraio.

**Q: Posso modificare una rendicontazione dopo averla inviata?**  
A: **No**. Una volta inviata, solo il Responsabile può approvarla o respingerla. Se respinta, dovrai crearne una nuova corretta.

**Q: Cosa succede se non invio la rendicontazione?**  
A: Le tue ore **non saranno fatturabili** e potresti non ricevere compensi per quel mese.

**Q: Posso creare più rendicontazioni per lo stesso mese?**  
A: Sì, ma solo **una alla volta** può essere "Inviata" o "Approvata". Se una viene respinta, puoi crearne una nuova.

**Q: Le note sono obbligatorie?**  
A: No, ma sono **consigliate** per comunicare informazioni utili al Responsabile.

**Q: Come faccio a sapere se è stata approvata?**  
A: Riceverai una **notifica via email** (se configurata) e vedrai lo stato cambiare in "Approvata" 🟢.

**Q: Posso vedere le rendicontazioni degli altri dipendenti?**  
A: **No**, puoi vedere solo le tue. Solo il Responsabile vede quelle di tutti.

**Q: Cosa fare se ho dimenticato ore straordinarie?**  
A: Se la rendicontazione è ancora in **Bozza**, modificala. Se già **Inviata**, contatta il Responsabile.

---

## Risoluzione Problemi

### Problema: Non vedo le mie attività nella rendicontazione
**Causa**: Le attività non sono state registrate nel calendario  
**Soluzione**: 
1. Vai su "Attività"
2. Registra le attività mancanti
3. Torna su "Rendicontazione"
4. Rigenera la rendicontazione

### Problema: Le ore totali non corrispondono
**Causa**: Errori di calcolo o attività duplicate  
**Soluzione**:
1. Controlla ogni singola attività nel calendario
2. Verifica che non ci siano duplicati
3. Ricalcola manualmente e confronta

### Problema: Non riesco a inviare la rendicontazione
**Causa**: Errore di rete o problema server  
**Soluzione**:
1. Controlla la connessione internet
2. Ricarica la pagina
3. Riprova tra qualche minuto
4. Contatta il supporto IT

---

## Checklist Mensile

Prima di inviare la rendicontazione, verifica:

- [ ] Tutte le giornate lavorate sono registrate nel calendario
- [ ] Ogni attività ha il progetto corretto
- [ ] Le descrizioni sono chiare e complete
- [ ] Gli orari sono corretti (inizio/fine)
- [ ] Le ore totali corrispondono alle aspettative
- [ ] Hai aggiunto note se necessario
- [ ] Hai controllato per duplicati

✅ **Se tutti i punti sono verificati, puoi inviare!**

---

## Supporto
Per assistenza:
- 📧 Email: supporto@azienda.it
- 📞 Telefono: +39 051 1234567
- 💬 Chat: #supporto-rendicontazione

---

**Ultima modifica**: Febbraio 2026  
**Versione**: 1.0
