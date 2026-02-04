# 📊 Progetti - Guida Responsabile

## Panoramica
Il modulo **Progetti** permette di creare, gestire e monitorare tutti i progetti aziendali, assegnare dipendenti e tracciare lo stato di avanzamento.

---

## Come Accedere
1. Login con credenziali Responsabile
2. Nella **sidebar**, clicca su **"Progetti"** (icona diagramma)
3. Visualizzerai tutti i progetti aziendali

**URL diretto**: `http://localhost:5178/Responsabile/Progetti`

---

## Funzionalità Principali

### 1️⃣ Creare un Nuovo Progetto

#### Step 1: Apri Modale
- Clicca su **"Nuovo Progetto"** (pulsante in alto a destra)

#### Step 2: Compila i Dati
**Campi obbligatori** (⭐):
- **Nome Progetto** ⭐: Titolo chiaro e identificativo
- **Cliente/Azienda** ⭐: Nome del cliente
- **Data Inizio** ⭐: Data di kick-off
- **Data Scadenza** ⭐: Deadline finale

**Campi opzionali**:
- **Descrizione**: Obiettivi e dettagli del progetto
- **Referente Cliente**: Contatto principale del cliente
- **Referente Interno**: Project Manager (solitamente tu)

**Esempio**:
```
Nome: Sistema ERP Aziendale
Cliente: Acme Corporation
Data Inizio: 01/03/2026
Data Scadenza: 30/09/2026
Descrizione: Sviluppo e implementazione di un sistema 
             ERP completo per gestione integrata
Referente Cliente: Laura Bianchi (l.bianchi@acme.com)
Referente Interno: Mario Rossi (Project Manager)
```

#### Step 3: Salva
- Clicca su **"Salva Progetto"**
- Il progetto appare nella lista

### 2️⃣ Assegnare Dipendenti a un Progetto

#### Perché Assegnare?
- I dipendenti vedono solo i **progetti assegnati**
- Possono registrare ore **solo** su progetti assegnati
- Definisci chi lavora su cosa

#### Come Assegnare
1. Nella card del progetto, clicca su **"Assegna"** (icona utente+)
2. Si apre il modale con l'elenco di **tutti i dipendenti**
3. **Seleziona** i dipendenti da assegnare (checkbox)
4. Clicca su **"Salva Assegnazioni"**

**Nota**: Puoi assegnare più dipendenti contemporaneamente

**Esempio di assegnazione**:
```
Progetto: Sistema ERP

Dipendenti assegnati:
☑️ Mario Verdi (Developer)
☑️ Luca Neri (Frontend)
☑️ Giulia Bianchi (QA Tester)
☐ Anna Rossi (Designer) - NON assegnata

Risultato: Solo Mario, Luca e Giulia vedranno 
questo progetto e potranno registrare ore
```

### 3️⃣ Modificare un Progetto

#### Campi Modificabili
- Nome, Cliente, Date, Descrizione, Referenti
- **Stato Completamento**: Attivo ↔ Completato

#### Come Modificare
1. Nella card del progetto, clicca su **"Modifica"** (icona matita)
2. Modifica i campi necessari
3. **Checkbox "Progetto Completato"**: Segna come finito
4. Clicca su **"Salva Progetto"**

**⚠️ Attenzione**: Segnare come "Completato" impedisce ai dipendenti di registrare nuove ore!

### 4️⃣ Eliminare un Progetto

#### Quando Eliminare?
- Progetto annullato
- Progetto creato per errore
- Pulizia vecchi progetti

#### Come Eliminare
1. Clicca su **"Elimina"** (icona cestino) nella card
2. Conferma l'eliminazione

**⚠️ ATTENZIONE**: 
- L'eliminazione è **permanente**
- Le ore già registrate potrebbero essere perse
- **Consigliato**: Segna come "Completato" invece di eliminare

---

## Gestione della Vista

### Filtri Disponibili

**Ricerca Testuale**:
- Cerca per nome progetto o cliente
- Ricerca in tempo reale

**Filtro Stato**:
- **Tutti**: Mostra tutti i progetti
- **In corso**: Solo progetti attivi
- **Completati**: Solo progetti finiti

**Ordinamento**:
- **Più recenti**: Per data inizio (più recente prima)
- **Per scadenza**: Progetti in scadenza prima
- **Per cliente**: Ordine alfabetico per cliente

### Informazioni Visualizzate

Per ogni progetto vedi:
- **Nome** e **Cliente**
- **Badge stato**:
  - 🟢 In Corso
  - 🟡 In Scadenza (< 7 giorni)
  - 🔴 Scaduto
  - ✅ Completato
- **Date**: Inizio e Scadenza
- **Descrizione** (se presente)
- **Referente Interno**
- **Pulsanti azione**: Assegna, Modifica, Elimina

---

## Workflow Completo

### Ciclo Vita Progetto

```
1. CREAZIONE
   Responsabile crea progetto
   ↓
2. ASSEGNAZIONE
   Assegna dipendenti al progetto
   ↓
3. LAVORO
   Dipendenti registrano ore nel calendario
   ↓
4. MONITORAGGIO
   Responsabile monitora avanzamento
   ↓
5. RENDICONTAZIONE
   Dipendenti inviano rendicontazioni mensili
   ↓
6. APPROVAZIONE
   Responsabile approva le ore
   ↓
7. FATTURAZIONE
   Responsabile fattura cliente (se billable)
   ↓
8. COMPLETAMENTO
   Progetto segnato come "Completato"
```

### Scenario: Nuovo Cliente e Progetto

**Situazione**: Cliente "Tech Solutions" richiede sviluppo app mobile

**Azione Responsabile**:

**Giorno 1 - Creazione**
1. Vai su "Progetti"
2. Clicca "Nuovo Progetto"
3. Compila:
   ```
   Nome: App Mobile E-Commerce
   Cliente: Tech Solutions
   Data Inizio: 01/04/2026
   Data Scadenza: 30/09/2026
   Descrizione: Sviluppo app mobile iOS/Android 
                per piattaforma e-commerce
   Referente Cliente: Marco Bianchi
   Referente Interno: Mario Rossi (te stesso)
   ```
4. Salva

**Giorno 2 - Assegnazione Team**
1. Apri progetto "App Mobile E-Commerce"
2. Clicca "Assegna"
3. Seleziona:
   - ☑️ Luca Verdi (Mobile Developer)
   - ☑️ Anna Neri (UX Designer)
   - ☑️ Giulia Rossi (QA Tester)
4. Salva assegnazioni

**Giorno 3 - Kickoff**
- Meeting con team
- Luca, Anna e Giulia vedono il progetto
- Iniziano a registrare ore

**Durante il progetto**:
- Monitora ore rendicontate
- Approva rendicontazioni mensili
- Fattura cliente mensilmente (se billable)

**30 Settembre - Fine Progetto**:
1. Vai su "Progetti"
2. Clicca "Modifica" su "App Mobile E-Commerce"
3. Segna ☑️ "Progetto Completato"
4. Salva

---

## Best Practices

### ✅ Cosa Fare

1. **Descrizioni dettagliate**
   - Spiega obiettivi e scope
   - I dipendenti devono capire cosa fare
   - Aggiungi link a documenti esterni se necessario

2. **Date realistiche**
   - Stima correttamente la durata
   - Considera buffer per imprevisti
   - Comunica cambiamenti di deadline

3. **Assegnazioni chiare**
   - Assegna solo chi deve lavorare sul progetto
   - Rivedi assegnazioni se il team cambia
   - Disassegna chi non è più coinvolto

4. **Monitora scadenze**
   - Controlla progetti in scadenza
   - Comunica tempestivamente con il team
   - Intervieni prima che scada

5. **Segna completati**
   - Quando finito, segna "Completato"
   - Mantieni la lista pulita
   - Non eliminare, solo marca come finito

### ❌ Cosa NON Fare

1. **Non lasciare progetti senza assegnazioni**
   - Progetto senza dipendenti = nessuno ci lavora
   - Verifica sempre le assegnazioni

2. **Non eliminare progetti con ore registrate**
   - Rischi di perdere dati
   - Usa "Completato" invece

3. **Non creare duplicati**
   - Controlla prima di creare
   - Un progetto = una entry

4. **Non ignorare progetti scaduti**
   - Badge rosso = problema
   - Intervieni: estendi deadline o chiudi

5. **Non assegnare troppi dipendenti**
   - Più persone ≠ più velocità
   - Team piccoli e focalizzati

---

## Monitoraggio e Reportistica

### Metriche Chiave

**Per Progetto**:
- Ore totali rendicontate
- Ore approvate vs non approvate
- Costo orario × ore = costo progetto
- Giorni alla scadenza
- Numero dipendenti assegnati

**Globali**:
- Progetti attivi totali
- Progetti in scadenza
- Progetti scaduti da chiudere
- Distribuzione ore per progetto

### Come Ottenere i Dati

**Ore rendicontate su un progetto**:
1. Vai su "Rendicontazione"
2. Filtra per dipendente
3. Visualizza dettaglio rendicontazione
4. Vedi ore per progetto

**Ore fatturabili**:
1. Vai su "Fatturazione"
2. Seleziona progetto
3. Vedi ore approvate fatturabili

---

## Integrazione con Altri Moduli

### Progetti → Attività (Calendario Dipendenti)
```
RESPONSABILE: Crea progetto "Sistema CRM"
    ↓
RESPONSABILE: Assegna Luca e Anna
    ↓
LUCA e ANNA: Vedono progetto nella loro lista
    ↓
LUCA: Registra attività su "Sistema CRM" nel calendario
ANNA: Registra attività su "Sistema CRM" nel calendario
```

### Progetti → Rendicontazione
```
RESPONSABILE: Assegna dipendenti a progetti
    ↓
DIPENDENTI: Lavorano e registrano ore
    ↓
FINE MESE
    ↓
DIPENDENTI: Inviano rendicontazioni
(Le rendicontazioni mostrano ore per progetto)
    ↓
RESPONSABILE: Approva rendicontazioni
```

### Progetti → Fatturazione
```
PROGETTI ATTIVI
    ↓
DIPENDENTI: Registrano ore approvate
    ↓
RESPONSABILE: Va su "Fatturazione"
    ↓
VEDE: Progetti con ore fatturabili
    ↓
GENERA: Fattura per cliente basata su ore × tariffa oraria
```

---

## Scenari Avanzati

### Scenario 1: Progetto Multipli Clienti
**Situazione**: Lavori per 3 clienti diversi contemporaneamente

**Gestione**:
```
Progetto A: ERP per Cliente Alpha
- Assegnati: Luca, Marco
- Durata: Gen-Giu 2026

Progetto B: CRM per Cliente Beta
- Assegnati: Anna, Giulia
- Durata: Feb-Ago 2026

Progetto C: Sito per Cliente Gamma
- Assegnati: Paolo (solo lui)
- Durata: Mar-Apr 2026
```

**Vantaggio**: Ogni cliente ha il suo progetto separato, ore fatturabili chiare

### Scenario 2: Progetto Lungo con Fasi
**Situazione**: Progetto di 12 mesi con milestone

**Approccio**:
```
Opzione A - Un Progetto con Note
Progetto: Piattaforma E-Commerce Completa
Descrizione:
  - Fase 1 (Gen-Mar): Design e mockup
  - Fase 2 (Apr-Giu): Sviluppo backend
  - Fase 3 (Lug-Set): Sviluppo frontend
  - Fase 4 (Ott-Dic): Testing e deploy

Opzione B - Progetti Separati per Fase
Progetto 1: E-Commerce - Fase Design (Gen-Mar)
Progetto 2: E-Commerce - Fase Backend (Apr-Giu)
Progetto 3: E-Commerce - Fase Frontend (Lug-Set)
Progetto 4: E-Commerce - Fase Testing (Ott-Dic)
```

**Consiglio**: Opzione A per progetti continui, Opzione B per fasi con team diversi

### Scenario 3: Cambio Team a Metà Progetto
**Situazione**: Luca lascia il progetto, entra Marco

**Azione**:
1. Vai su "Progetti"
2. Apri progetto
3. Clicca "Assegna"
4. Deselezione Luca ☐
5. Seleziona Marco ☑️
6. Salva

**Risultato**:
- Luca: Non vede più il progetto (ma ore già registrate rimangono)
- Marco: Vede il progetto da ora in poi

---

## FAQ

**Q: Posso avere più progetti per lo stesso cliente?**  
A: **Sì**, crea un progetto separato per ogni iniziativa.

**Q: Cosa succede alle ore se elimino un progetto?**  
A: Le ore già registrate potrebbero essere perse. **Meglio**: Segna come "Completato" invece di eliminare.

**Q: Posso assegnare un dipendente a metà progetto?**  
A: **Sì**, in qualsiasi momento. Basta cliccare "Assegna" e selezionarlo.

**Q: Come faccio a vedere chi lavora su un progetto?**  
A: Clicca "Assegna" sul progetto, vedrai l'elenco con checkbox già selezionati per chi è assegnato.

**Q: Posso modificare un progetto già iniziato?**  
A: **Sì**, puoi modificare tutto tranne eliminare (sconsigliato se ci sono ore registrate).

**Q: Cosa significa "Progetto Completato"?**  
A: Il progetto è finito. I dipendenti **non possono** più registrare ore. Le ore già registrate rimangono valide.

**Q: Posso riattivare un progetto completato?**  
A: **Sì**, vai su "Modifica" e deseleziona "Progetto Completato".

---

## Risoluzione Problemi

### Problema: Dipendente non vede il progetto
**Causa**: Non è stato assegnato  
**Soluzione**: Vai su progetto → Assegna → Seleziona dipendente → Salva

### Problema: Dipendente non può registrare ore
**Causa 1**: Progetto segnato come "Completato"  
**Soluzione**: Riattiva il progetto (Modifica → Deseleziona Completato)

**Causa 2**: Dipendente non assegnato  
**Soluzione**: Assegna il dipendente

### Problema: Troppe richieste di assegnazione
**Causa**: Dipendenti vogliono lavorare su progetti non assegnati  
**Soluzione**: Valuta caso per caso, assegna se necessario, spiega la policy se rifiuti

---

## Supporto
Per assistenza tecnica:
- 📧 Email: admin@azienda.it
- 📞 Telefono: +39 051 1234567

---

**Ultima modifica**: Febbraio 2026  
**Versione**: 1.0
