Feature: Payment succeeds

  As a customer
  I want my payment to be processed successfully
  So that my order is confirmed and sent to the kitchen

  Scenario: Customer places an order and payment succeeds
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Classic Milk Tea" with default options
    Then they receive a confirmation with an order number

  Scenario: Paid order is visible on the kitchen display
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Classic Milk Tea" with default options
    Then the order appears on the kitchen display
