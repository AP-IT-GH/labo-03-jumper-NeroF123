## Project Documentatie: Obstakel Ontwijkende Springer

> **Studenten**: Brusselaers Senne (S138685), De Laet Thijs (151582)

### Implementatie Overzicht:

1.  Opzetten van een plane met een agent en een obstakel .

2.  Het obstakel continu naar de agent laten bewegen.

3.  De agent de mogelijkheid geven om te springen.

4.  Een beloningssysteem implementeren waarbij de agent een negatieve
    beloning krijgt als hij geraakt wordt door het obstakel, en ook een
    kleine negatieve beloning voor springen.

5.  De agent belonen voor elke stap die hij overleeft en voor het
    succesvol voltooien van een episode door het obstakel te ontwijken.

> Wij hebben gekozen voor de uitwerking van de eerste functionaliteit
> namelijk de agent wordt geconfronteerd met een rij van continu
> bewegende obstakels.

- Set-up: Een lange, vlakke omgeving. De agent wordt in het midden
  geplaatst. Een bewegend obstakel start aan één kant van het vlak en
  beweegt richting de agent. Als de agent succesvol over het obstakel
  springt, reset het obstakel naar zijn startpositie en beweegt opnieuw
  naar de agent.Goal: Ontwijk de bewegende balk.

- Goal : De agent moet leren om over het continu bewegende obstakel te
  springen om zo lang mogelijk te overleven binnen de tijdslimiet van
  een episode.

- Agents: De omgeving bevat 1 agent.

- Agent Reward Function:

  - -0.05 voor elke jump

  - -2.0 alse de agent bots met het obstakel (Beëindigt de episode)

  - +0.001 voor elke stap die de agent overleeft

  - +1.0 voor elke episode die voltooid wordt zonder geraakt te worden

- Behavior Parameters:

  - Vector Observation space:

    - Agent Y-locatie: Alleen de Y-locatie van de agent moet doorgegeven
      worden doordat de agent niet langs de X en Z-axis kan bewegen

    - Obstacle locatie: De X , Y en Z-locaties worden meegegeven zodat
      de agent weet hoe ver het obstakel van hem verwijderd is.

    - IsGrounded variabele: een boolean die vertelt aan de agent of die
      op de grond staat of niet.

  - Actions: 1 discrete actie branch met 2 actions (Spring, doe niks).

  - Visual Observations: RayPerception Sensor van first person POV.

- Float Properties: Geen

- Benchmark Mean Reward: 3.98 (bereikt na 1 miljoen stappen)

(![images/cumulative_reward_plot.png](https://github.com/AP-IT-GH/labo-03-jumper-NeroF123/blob/main/config/results/Test3/image.png))

Deze grafiek toont de leercurve van de agent in termen van het maximale score per episode. 

In het begin is de beloning laag en vaak negatief (start rond -1) dit komt door frequente botsingen (-2.0 penalty) en onnodige sprongen (-0.05 penalty). 

Vanaf ongeveer 0k tot 350k stappen is er een duidelijke stijgende lijn, hoewel met schommelingen, wat aangeeft dat de agent leert het obstakel effectiever te ontwijken. 

Na ongeveer 350k-400k stappen stabiliseert de cumulatieve beloning en bereikt een plateau rond een waarde van 4.0. Dit plateau geeft aan dat de agent een “correcte” oplossing heeft gevonden.  


(![images/Episode Length.png](https://github.com/AP-IT-GH/labo-03-jumper-NeroF123/blob/main/config/results/Test3/image2.png))

Deze grafiek toont hoe lang een episode duurt, gemeten in het aantal stappen dat de agent overleeft.
In het begin zijn de episodes kort (50-100 stappen), omdat de agent nog niet geleerd heeft het obstakel te ontwijken en snel botst.

Vanaf ongeveer 0k tot 350k stappen is er een duidelijke stijging in de episodelengte, wat betekent dat de agent steeds langer weet te overleven.

Na ongeveer 350k-400k stappen stabiliseert de episodelengte rond de maximale waarde van 300 stappen. Dit geeft aan dat de agent consistent het obstakel kan ontwijken gedurende de volledige toegestane tijd van een episode (60 seconden).



