Feature: Customise drink

  As a customer
  I want to customise my drink
  So that I get exactly what I want and see the correct price

  Scenario: Default customisation gives base price
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Classic Milk Tea" with default options
    Then the total price is $5.50

  Scenario: Oat milk adds to the price
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Classic Milk Tea" with oat milk
    Then the total price is $6.00

  Scenario: Each topping adds to the price
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Taro Milk Tea" with tapioca pearls
    Then the total price is $6.75

  Scenario: Full customisation — Taro with oat milk and two toppings
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Taro Milk Tea" with oat milk, tapioca pearls and coconut jelly
    Then the total price is $8.00

  Scenario: More than two toppings is rejected
    Given the customer is at the kiosk
    When they try to order a "Classic Milk Tea" with three toppings
    Then the order is rejected
