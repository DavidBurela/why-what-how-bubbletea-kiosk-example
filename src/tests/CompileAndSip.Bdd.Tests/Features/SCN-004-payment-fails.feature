Feature: Payment fails

  As a customer
  I want to be told clearly when payment fails
  So that I can try again or pay at the counter

  Scenario: Payment is declined
    Given the customer is at the kiosk
    And the payment will be declined
    When they order a "Classic Milk Tea" with default options
    Then they are told the payment did not go through
    And no order is sent to the kitchen

  Scenario: Payment provider is unavailable
    Given the customer is at the kiosk
    And the payment provider is unavailable
    When they order a "Classic Milk Tea" with default options
    Then they are advised to pay at the counter
    And no order is sent to the kitchen
