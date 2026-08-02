# Market and economy

**Status:** Canonical direction; numerical balancing remains open
**Last reviewed:** 2026-08-02

## Core rule

> The market is a network of named people and commitments, not a universal sell box.

Money creates pressure around patience, inventory, promises, and identity. It supports the craft rather than replacing it with a pure tycoon game.

## Economic loop

```text
cash and relationships
    -> leaf, wages, facilities, and time
    -> crops, processed lots, and cigars
    -> allocation and release decisions
    -> orders, payments, feedback, and reputation
    -> better access and new opportunities
```

The central tension is deciding what the house can afford to wait for.

## Market accounts

Every buyer or commercial partner is a named account connected to a character or organization. An account can have:

- Client or audience preferences
- Typical price position
- Order capacity and shelf space
- Payment reliability and terms
- Tolerance for experimental work
- Expectations about construction and consistency
- Regional or channel reach
- Current inventory and recent sell-through
- Trust, communication, and contract history

A cigar can be excellent but poorly matched to an account's clientele. The player learns where each expression belongs.

## Sales channels

| Channel | Advantage | Cost or limitation |
| --- | --- | --- |
| Leaf broker | Fast cash from cured and graded crops | Gives up future blending material |
| Visiting buyer | Personal feedback and early access | Small, irregular orders |
| Private commission | Strong margin and relationship | Tiny volume and demanding expectations |
| Specialty retailer | Clear identity and useful customer feedback | Small accounts requiring attention |
| Importer or distributor | Reliable access to larger markets | Lower margins and less downstream control |
| Private-label contract | Stable income and production experience | Consumes capacity without building the house name |
| Festival or exhibition | Attention and new relationships | Expense and uncertain direct sales |
| Collector allocation | Prestige and high value | Very small volume and scarcity expectations |

Channels unlock through relationships, capability, and trust rather than a linear business level.

## Release lifecycle

```text
draft
  -> sampling
  -> offered
  -> ordered
  -> inventory allocated
  -> packaged
  -> shipped
  -> in market
  -> payment settled
  -> reordered or archived
```

Shipping is not the same as market success. Buyer acceptance, payment, consumer sell-through, feedback, and reorders occur at different times.

## Release decisions

For each release, the player chooses:

- Approved cigar and recipe version
- Source batch and aging selection
- Quantity released, sampled, and held in reserve
- Packaging format and presentation
- Wholesale or ex-finca price
- Buyer allocations
- Exclusivity, if offered
- Deposit and payment terms
- Promises about delivery, provenance, and consistency
- Intended audience and house positioning

A batch can be divided. Some boxes may ship while others continue aging as a reserve.

## Inventory commitment

Inventory becomes unavailable when contractually allocated. The player cannot promise the same leaf, cigars, or production capacity to several accounts.

Reservations distinguish:

- Internal recipe commitments
- Samples
- Confirmed customer orders
- Contract production
- Private reserves
- Unallocated sale inventory

Breaking an allocation requires an explicit decision and creates appropriate commercial consequences.

## Demand

Demand is resolved separately for each account:

```text
audience fit
* buyer trust
* price fit
* product awareness
* relevant house reputation
* prior performance
* available shelf capacity
= likely order range
```

The player sees a forecast with reasons and uncertainty, not an exact hidden demand value.

An offer can lead to confirmation, negotiation, a smaller trial order, a request for samples, postponement, or refusal. Demand saturates: one enthusiastic account cannot absorb infinite inventory, and flooding a market can produce unsold stock or weaken scarcity.

## Sell-through and feedback

After shipment, reports may reveal:

- Speed of customer sell-through
- Audience response by style or price
- Construction complaints and returns
- Requests for another vitola or presentation
- Reorders and waiting lists
- Discounting or unsold stock
- Interest from other accounts

Feedback can be delayed, incomplete, or shaped by the account's communication quality. It updates knowledge rather than revealing one universal verdict.

## Pricing

The player controls the price received by the house, not every downstream retail transaction.

- Low pricing may generate orders but leave insufficient margin or weaken premium positioning.
- High pricing restricts access and raises expectations.
- Scarcity may support price, but artificial withholding can damage trust.
- Established portfolio cigars require greater price consistency than one-time atelier releases.
- Private commissions and rare releases do not automatically redefine the entire portfolio's price.

Buyers respond through negotiation, order size, payment terms, or refusal. The game does not calculate one perfect price for the player.

## Contracts

A contract records:

- Named counterparty
- Product, leaf lot, or production specification
- Quantity and delivery window
- Unit or batch price
- Deposit and payment schedule
- Packaging and labeling requirements
- Quality tolerances
- Permitted substitutions
- Regional or channel exclusivity
- Consequences for missed commitments

Supported contract types include one-time release orders, recurring portfolio orders, private-label production, bespoke commissions, leaf sales, and advance crop agreements.

Contracts exchange flexibility for security. A valuable recurring order can consume leaf, labor, or aging space the player wanted for personal work.

## Ledger and currency

The game uses one accounting currency. Exact denomination and visual presentation follow the final setting decision; foreign-exchange speculation is outside scope.

The player-facing ledger emphasizes:

- Available cash
- Upcoming obligations
- Wages and recurring costs
- Outstanding payments
- Contracted future income
- Committed inventory
- Expected near-term cash position

Leaf, aging tobacco, and finished cigars possess value but are not spendable cash.

## Cost history

Every lot and production batch can accumulate traceable direct costs:

- Tobacco or crop inputs
- Labor
- Processing
- Storage
- Packaging
- Shipping
- Contract or export expenses

This supports pricing and post-release analysis without requiring formal accounting knowledge. Shared overhead can remain summarized at the house level.

## Financial rhythm

- Immediate purchases affect cash when approved.
- Wages and ordinary operating expenses settle predictably.
- Contracts define deposits and payment dates.
- Major construction requires explicit approval and funding.
- Upcoming obligations appear on the calendar before they become urgent.

Do not surprise the player with unexplained deductions or require attention to a stream of trivial daily charges.

## Early-game economy

The player begins with a modest reserve, a neglected finca, a small amount of usable or purchased tobacco, and one account willing to consider a tiny order.

Early survival can combine:

- Selling part of the cured estate crop
- Accepting limited private-label work
- Producing tiny house releases
- Completing private commissions
- Delaying improvements
- Holding only the most promising inventory for long aging

The first house-branded box should matter financially and emotionally.

## Financial distress and recovery

Financial pressure is challenging but recoverable. Distress progresses visibly:

1. Forecasted shortfall
2. Tight cash and delayed optional work
3. Negotiation, credit, contract, or asset-sale opportunities
4. Missed obligations and relationship damage
5. Sustained insolvency only after several ignored recovery paths

Recovery options can include selling a reserved lot, releasing part of a batch early, renegotiating payment, taking less prestigious contract work, using credit, postponing construction, or seeking an advance from a trusted account.

One poor release or season does not abruptly delete the player's progress. Serious collapse remains possible only after repeated warnings and meaningful choices.

## Supported business identities

The system must support:

- A tiny atelier with rare, expensive releases
- A balanced boutique house
- A dependable portfolio producer
- A contract manufacturer funding personal work
- A respected grower selling exceptional tobacco
- A larger premium house with selective distribution

None is merely an incomplete version of the largest operation.

## Conceptual entities

Implementation should preserve distinct state for:

- Market account and audience profile
- Release
- Offer and negotiation
- Order and allocation
- Contract
- Shipment
- Receivable and payment
- Ledger entry
- Sell-through report
- Commercial feedback event

## Abstraction boundaries

- Export, tax, logistics, and regulation should create understandable costs, requirements, and risks without recreating real legal paperwork.
- Real-world rates and current laws must not be hard-coded as timeless game balance.
- Exact prices, wages, order sizes, payment periods, and operating costs are balancing data.
- The market simulation should use accounts and audience segments rather than simulating every individual consumer.

## Design constraints

- Never provide an infinite anonymous buyer.
- Never convert every finished batch instantly into cash.
- Never let one global demand number erase audience fit.
- Preserve delayed payment and sell-through as distinct events.
- Make committed inventory and capacity impossible to double-spend.
- Keep financial pressure legible and recoverable.
- Ensure that commercial success does not automatically equal critical or cultural prestige.

## Research anchors

- [Nicaragua Free Zones Commission sessions](https://cnzf.gob.ni/sesiones/) - tobacco pre-processing, manufacturing, leaf export, and finished-cigar export activity in Estelí
- [Central Bank of Nicaragua annual report 2025](https://www.bcn.gob.ni/publicaciones/informe-anual-2025-0) - tobacco products within the country's manufacturing and export economy
- [Barreda Cigars services](https://barredacigars.com/es/services/) - a current producer example connecting private-label work, export support, and distribution
