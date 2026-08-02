# Flavor and sensory model

**Status:** Canonical direction; numerical calibration remains open
**Last reviewed:** 2026-08-02

## Core rule

> A cigar has an underlying physical expression, but every tasting is an interpretation of that expression.

The simulation produces stable, explainable results without declaring one person's tasting vocabulary objectively correct. Flavor is neither an arbitrary roll nor a perfectly visible stat block.

## Experience model

```text
provenance + processing
        -> underlying leaf expression
        -> blend + construction + aging
        -> cigar expression over time
        -> taster knowledge + sensitivity + context
        -> observation and judgment
```

## Data layers

### Leaf potential

Each provenance-rich lot carries hidden tendencies for physical behavior, strength, body, broad aroma families, sweetness or dryness, irritation, and finish. Potential describes a range, not a finished flavor.

### Process state

Curing, conditioning, fermentation, resting, aging, moisture, damage, and storage determine how much of the potential is preserved, transformed, muted, or made unpleasant.

### Cigar expression

Blend interactions, component condition, vitola, construction, airflow, combustion, and aging produce a changing experience across the cigar.

### Taster observation

Every taster samples that expression through their own experience, sensitivity, vocabulary, expectations, preferences, and context. Observations have confidence; they are not direct access to hidden simulation values.

## Core experience dimensions

These dimensions remain separate throughout the data model:

- **Strength:** Physiological impact
- **Body:** Perceived weight and density of the smoke
- **Flavor intensity:** How prominently sensory character presents
- **Sweetness and dryness:** Perceived taste balance, distinct from sweet-smelling aromas
- **Smoothness and irritation:** Softness, sharpness, heat, or harshness
- **Finish length:** How long impressions remain after the smoke leaves the mouth
- **Smoke temperature:** A construction and smoking-condition result that changes perception

The interface uses descriptive ranges such as mild, medium, or full rather than exposing raw normalized values.

## Aroma families

Specific descriptors belong beneath broader families:

- **Earth and mineral:** Soil, stone, mushroom
- **Wood:** Cedar, oak, dry wood
- **Spice:** Pepper, baking spice, chili
- **Roasted and nutty:** Cocoa, coffee, toast, nuts
- **Sweet and baked:** Honey, caramel, molasses, bread
- **Fruit:** Citrus, dried fruit, dark fruit
- **Floral:** Flowers, perfume, tea blossom
- **Herbal and green:** Grass, hay, herbs, tea
- **Fermented and leather:** Leather, musk, aged or fermented character

This is a controlled game vocabulary, not a claim that every perception has one chemical cause. Descriptor wording can expand through research and testing without changing the family model.

Perceived sweetness and a sweet-associated aroma are distinct. A cigar can suggest cocoa or molasses while remaining dry on the palate.

## Evolution

The underlying expression may change continuously, while the journal summarizes five useful checkpoints:

1. Pre-light condition and aroma
2. Opening and first portion
3. Middle portion
4. Final portion
5. Finish and aftertaste

Evolution can be described as static, gradual, layered, building, fading, dramatic, or disjointed. The player influences an arc through materials, proportions, geometry, combustion, and processing; they do not assign an exact flavor to a geometric third.

## Knowledge and confidence

The game never randomly lies because the player lacks a high enough palate level. Expertise improves:

- Descriptor precision
- Confidence ranges
- Recognition of transitions
- Separation of strength, body, intensity, and heat
- Recognition of construction versus blend problems
- Causal hypotheses about processing, aging, and component behavior

A beginner might record warm sweetness, earth, and a sharp finish. An experienced player might distinguish cocoa-like roast from sweetness, notice pepper increasing with smoke temperature, and identify several plausible causes.

## What the player knows about a lot

Before testing, knowledge may come from:

- The grower's or seller's description
- Provenance and agricultural records
- Physical inspection
- Previous crops from the field or grower
- Prior use of related lots
- The player's own tests and tasting history

The lot view distinguishes reported claims, direct observations, predictions, and confirmed production history. Repeated experience narrows uncertainty without making different harvests identical.

## Evaluation is not one score

| Evaluation | Question |
| --- | --- |
| Technical integrity | Is the cigar well constructed, conditioned, and free of serious defects? |
| Design coherence | Do its characteristics work together? |
| Intent fidelity | Does it deliver the experience the maker intended? |
| Consistency | Does the batch reproduce the approved prototype? |
| Preference | Does this particular person enjoy this style? |
| Distinctiveness | Does it possess a memorable identity? |

A cigar can be technically excellent and faithful to its intent while being disliked by a particular audience. Market response, critical respect, and craft quality must remain related but distinct.

## Implementation constraints

- Do not store or expose one universal cigar-quality value.
- Do not treat aroma families as ingredients whose points merely add together.
- Do not make later or rarer tobacco strictly superior.
- Preserve the hidden distinction between expression and perception.
- Make identical inputs and process histories deterministic apart from controlled, saved batch variation.
- Keep the first implementation compact enough to tune before expanding the descriptor vocabulary.

## Research anchors

- [Habanos: factory tasters](https://www.habanos.com/en/news/20-de-noviembre-dia-del-catador-en-cuba-en/) - separate evaluation of draw, combustion, aroma, flavor, strength, and overall result
- [Habanos: choosing and smoking](https://www.habanos.com/en/choosing-cutting-lighting-and-smoking/) - condition, smoking pace, and staged intensification
- [Habanos: shapes and sizes](https://www.habanos.com/en/principal-shapes-sizes/) - format changing temperature, concentration, and experienced blend
- [Molecular sensory analysis of cigar tobacco](https://pmc.ncbi.nlm.nih.gov/articles/PMC12287006/) - structured sensory terms and regional profile differences
- [Fermentation microflora and flavor substances](https://pmc.ncbi.nlm.nih.gov/articles/PMC10699171/) - complex relationships among volatile compounds, aroma thresholds, and perception
