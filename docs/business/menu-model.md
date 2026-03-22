# Menu

Compile & Sip's drink catalogue, customisation options, and pricing rules. This is black-box domain knowledge describing what the shop sells and how customisation works.

## Drinks

| # | Drink Name | Description | Base Price |
|---|-----------|-------------|------------|
| 1 | Classic Milk Tea | Traditional black tea with milk and tapioca pearls. The original. | $5.50 |
| 2 | Taro Milk Tea | Creamy taro root blended with milk tea. Rich and slightly sweet. | $6.00 |
| 3 | Mango Green Tea | Fresh mango with jasmine green tea. Light and refreshing. | $6.00 |

## Customisation options

| Category | Options | Default | Price Impact |
|----------|---------|---------|-------------|
| **Milk** | Regular, Oat, None | Regular | Oat: +$0.50, None: no change |
| **Toppings** (pick 0–2) | Tapioca Pearls, Coconut Jelly, Pudding | None | $0.75 each |
| **Sugar Level** | 0%, 25%, 50%, 75%, 100% | 100% | No change |
| **Ice Level** | No Ice, Less Ice, Regular Ice | Regular Ice | No change |
| **Temperature** | Hot, Cold | Cold | No change |

## Pricing rules

The total price is calculated as:

**Base price + milk modifier + topping charges**

- Base price is set per drink (see table above).
- Oat milk adds $0.50. Regular and None have no additional charge.
- Each topping adds $0.75. Customers may select 0 to 2 toppings.
- Sugar level, ice level, and temperature have no price impact.

### Example

Taro Milk Tea + Oat Milk + Tapioca Pearls + Coconut Jelly = $6.00 + $0.50 + $0.75 + $0.75 = **$8.00**
