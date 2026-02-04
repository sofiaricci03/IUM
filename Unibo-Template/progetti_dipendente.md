# 📁 Progetti - Guida Dipendente

## Panoramica
Il modulo **Progetti** permette di visualizzare i progetti a cui sei stato assegnato, con dettagli su scadenze, clienti e descrizioni.

---

## Come Accedere
1. Login con le tue credenziali
2. Nella **sidebar**, clicca su **"Progetti"** (icona cartella)
3. Visualizzerai l'elenco dei tuoi progetti

**URL diretto**: `http://localhost:5178/Dipendente/Progetti`

---

## Funzionalità Principali

### 1️⃣ Visualizzare i Progetti Assegnati

Nella pagina principale vedi una **lista di card** con:

#### Informazioni per Progetto
- **Nome Progetto**: Titolo del progetto
- **Cliente**: Azienda o cliente finale
- **Stato**: Attivo / Completato
- **Date**: 
  - Data Inizio
  - Data Scadenza
- **Descrizione**: Breve descrizione obiettivi
- **Referenti**:
  - Referente Interno (Project Manager)
  - Referente Cliente (se disponibile)

#### Badge di Stato
- 🟢 **Attivo**: Progetto in corso
- 🔵 **Completato**: Progetto finito
- ⚠️ **In Scadenza**: Meno di 7 giorni alla deadline
- 🔴 **Scaduto**: Oltre la data di scadenza

**Esempio di card progetto**:
```
┌─────────────────────────────────────┐
│ 📊 Sistema ERP Aziendale            │
│ 🟢 Attivo                           │
├─────────────────────────────────────┤
│ Cliente: Acme Corporation           │
│ Inizio: 01/01/2026                  │
│ Scadenza: 30/06/2026                │
│                                     │
│ Sviluppo e implementazione di un    │
│ sistema ERP completo per la         │
│ gestione aziendale integrata.       │
│                                     │
│ 👤 PM: Mario Rossi                  │
│ 📞 Cliente: Laura Bianchi           │
└─────────────────────────────────────┘
```

### 2️⃣ Filtrare e Cercare

**Barra di ricerca**: Cerca per nome progetto o cliente

**Filtri disponibili**:
- **Stato**: 
  - Tutti
  - Solo Attivi
  - Solo Completati
- **Ordinamento**:
  - Per scadenza (più urgenti prima)
  - Per nome (A-Z)
  - Per cliente

### 3️⃣ Dettagli Progetto

Cliccando su un progetto, puoi vedere:
- **Descrizione completa**
- **Obiettivi**
- **Milestone** (se disponibili)
- **Team**: Altri dipendenti assegnati
- **Documenti** (se allegati)

---

## Capire i Progetti

### Perché Sei Assegnato a un Progetto?

Quando il Responsabile ti assegna a un progetto, significa che:
1. ✅ Puoi **registrare ore** su quel progetto nel calendario
2. ✅ Hai **accesso alle informazioni** del progetto
3. ✅ Sei parte del **team** di lavoro
4. ✅ Il Responsabile si aspetta il tuo **contributo**

### Come Utilizzare le Informazioni

**Per la registrazione attività**:
- Consulta i progetti assegnati per sapere su cosa lavorare
- Verifica le scadenze per prioritizzare
- Leggi la descrizione per capire gli obiettivi

**Per la pianificazione**:
- Controlla le date di scadenza
- Coordina con il team
- Pianifica le tue ore in base alle priorità

---

## Workflow Tipico

### Scenario: Nuovo Progetto Assegnato

```
RESPONSABILE
    ↓
Crea progetto "Sistema CRM"
    ↓
Assegna te e altri 2 colleghi
    ↓
NOTIFICA
    ↓
VAI SU "PROGETTI"
    ↓
Vedi nuovo progetto nella lista
    ↓
Clicca per vedere dettagli:
- Cliente: Tech Solutions
- Scadenza: 30/09/2026
- Descrizione: Sviluppo CRM custom
- Team: Tu, Luca, Giulia
    ↓
Prendi nota e inizia a lavorare
    ↓
REGISTRA ORE nel calendario
```

### Integrazione con Altri Moduli

#### Progetti → Attività
```
1. Consulti la lista progetti
2. Scegli su quale progetto lavorare oggi
3. Vai su "Attività" (calendario)
4. Registri attività selezionando quel progetto
```

#### Progetti → Rendicontazione → Fatturazione
```
DIPENDENTE: Lavora su progetto e registra ore
    ↓
DIPENDENTE: Invia rendicontazione mensile
    ↓
RESPONSABILE: Approva rendicontazione
    ↓
RESPONSABILE: Usa ore approvate per fatturare cliente
```

---

## Best Practices

### ✅ Cosa Fare

1. **Consulta regolarmente**
   - Controlla i progetti assegnati all'inizio settimana
   - Verifica le scadenze imminenti

2. **Leggi le descrizioni**
   - Comprendi gli obiettivi del progetto
   - Allineati con il team

3. **Comunica con il PM**
   - Se hai dubbi, contatta il referente interno
   - Aggiorna il PM sui progressi

4. **Prioritizza in base a scadenza**
   - Lavora prima sui progetti in scadenza
   - Bilancia il tempo tra progetti multipli

### ❌ Cosa NON Fare

1. **Non registrare ore su progetti non assegnati**
   - Puoi lavorare solo sui progetti assegnati
   - Se vuoi lavorare su altro, chiedi al Responsabile

2. **Non ignorare le scadenze**
   - Le date di scadenza sono importanti
   - Pianifica il lavoro per rispettarle

3. **Non lavorare senza capire gli obiettivi**
   - Leggi la descrizione
   - Chiedi chiarimenti se necessario

---

## Scenari Comuni

### Scenario 1: Primo Giorno di Lavoro
**Situazione**: È il tuo primo giorno, non sai su cosa lavorare

**Azione**:
1. Vai su "Progetti"
2. Vedi i progetti assegnati
3. Leggi descrizioni e obiettivi
4. Contatta il Responsabile per chiarimenti
5. Inizia a lavorare sul progetto prioritario

### Scenario 2: Multipli Progetti Attivi
**Situazione**: Sei assegnato a 4 progetti contemporaneamente

**Come gestirli**:
1. **Identifica priorità**:
   - Progetto A: Scadenza 31/03 → Priorità ALTA
   - Progetto B: Scadenza 30/06 → Priorità MEDIA
   - Progetto C: Scadenza 31/12 → Priorità BASSA
   - Progetto D: Manutenzione → CONTINUA

2. **Distribuisci il tempo**:
   - 50% su Progetto A (più urgente)
   - 25% su Progetto B
   - 15% su Progetto D
   - 10% su Progetto C

3. **Registra ore correttamente**:
   - Nel calendario, seleziona il progetto giusto
   - Non accumulare ore su un solo progetto

### Scenario 3: Progetto Non Chiaro
**Situazione**: Sei stato assegnato ma non capisci gli obiettivi

**Azione**:
1. Leggi attentamente la descrizione
2. Consulta documenti allegati (se presenti)
3. Contatta il Referente Interno (PM)
4. Chiedi un meeting di allineamento
5. Prendi appunti e inizia a lavorare

### Scenario 4: Progetto Completato
**Situazione**: Il progetto è stato segnato come "Completato"

**Cosa significa**:
- ✅ Il progetto è finito
- ❌ NON puoi più registrare ore su quel progetto
- 📊 Le ore già registrate rimangono valide

**Cosa fare**:
- Verifica che tutte le tue ore siano registrate
- Passa agli altri progetti attivi
- Se devi ancora lavorarci, contatta il Responsabile

---

## Capire le Scadenze

### Calcolo Giorni Rimanenti

Il sistema mostra automaticamente:
```
Progetto: Sistema ERP
Scadenza: 30/06/2026
Oggi: 15/06/2026

Giorni rimanenti: 15 giorni
Stato: ⚠️ IN SCADENZA
```

### Badge di Urgenza

| Giorni Rimanenti | Badge | Colore | Azione |
|-----------------|-------|---------|--------|
| > 30 giorni | 🟢 In Corso | Verde | Procedi normale |
| 8-30 giorni | 🟡 Da Monitorare | Giallo | Aumenta focus |
| 1-7 giorni | ⚠️ In Scadenza | Arancione | Priorità ALTA |
| 0 giorni (scaduto) | 🔴 Scaduto | Rosso | URGENTE! |

### Cosa Fare per Ogni Badge

**🟢 In Corso**:
- Lavora con ritmo normale
- Monitora progressi settimanali

**🟡 Da Monitorare**:
- Aumenta il focus
- Verifica se sei in linea con gli obiettivi
- Comunica al PM eventuali problemi

**⚠️ In Scadenza**:
- Priorità massima
- Sprint finale
- Comunica stato avanzamento
- Chiedi supporto se necessario

**🔴 Scaduto**:
- Contatta IMMEDIATAMENTE il Responsabile
- Spiega la situazione
- Concorda nuovo piano

---

## FAQ

**Q: Posso registrare ore su progetti non assegnati?**  
A: **No**. Puoi lavorare solo sui progetti a cui sei stato assegnato. Se vuoi lavorare su un progetto non in lista, chiedi al Responsabile di assegnarti.

**Q: Come faccio a sapere su quale progetto lavorare?**  
A: Controlla le scadenze e le priorità. In caso di dubbio, chiedi al Responsabile o al Project Manager.

**Q: Posso vedere progetti di altri dipendenti?**  
A: **No**, vedi solo i tuoi progetti assegnati. Solo il Responsabile vede tutti i progetti aziendali.

**Q: Cosa fare se un progetto non ha scadenza?**  
A: Chiedi chiarimenti al Responsabile. Ogni progetto dovrebbe avere una timeline chiara.

**Q: Posso essere assegnato a un progetto a metà strada?**  
A: **Sì**, il Responsabile può assegnarti in qualsiasi momento se serve il tuo contributo.

**Q: Cosa succede se non rispetto una scadenza?**  
A: Comunica tempestivamente al Responsabile. Insieme troverete una soluzione (es: estensione deadline, supporto extra team).

---

## Glossario

- **Progetto**: Iniziativa aziendale con obiettivi, scadenze e team
- **Assegnazione**: Quando il Responsabile ti assegna a un progetto
- **PM (Project Manager)**: Referente interno del progetto
- **Cliente**: Azienda o persona per cui si lavora
- **Milestone**: Obiettivo intermedio da raggiungere
- **Scadenza**: Data entro cui il progetto deve essere completato
- **Ore fatturabili**: Ore approvate che possono essere addebitate al cliente

---

## Risoluzione Problemi

### Problema: Non vedo nessun progetto
**Causa**: Non sei stato assegnato a nessun progetto  
**Soluzione**: Contatta il Responsabile per farti assegnare

### Problema: Vedo un progetto ma non riesco a registrare ore
**Causa**: Il progetto potrebbe essere completato o disabilitato  
**Soluzione**: Verifica lo stato del progetto. Contatta il Responsabile se necessario

### Problema: Non capisco la descrizione del progetto
**Causa**: Descrizione poco chiara o mancante  
**Soluzione**: Contatta il Referente Interno (PM) per chiarimenti

### Problema: Troppe ore richieste su troppi progetti
**Causa**: Sovraccarico di lavoro  
**Soluzione**: 
1. Parla con il Responsabile
2. Richiedi supporto o redistribuzione del carico
3. Prioritizza i progetti più urgenti

---

## Supporto
Per assistenza:
- 📧 Email: supporto@azienda.it
- 📞 Telefono: +39 051 1234567
- 💬 Chat: #supporto-progetti

---

**Ultima modifica**: Febbraio 2026  
**Versione**: 1.0
