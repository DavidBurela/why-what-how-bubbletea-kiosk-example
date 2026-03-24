Feature: Kitchen marks order complete

  As kitchen staff
  I want to mark orders as complete
  So that finished orders are removed from the active list

  Scenario: Paid order is visible with full details
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Taro Milk Tea" with oat milk, tapioca pearls and coconut jelly
    Then the kitchen display shows the order with drink "Taro Milk Tea" at $8.00

  Scenario: Staff marks an order complete
    Given the customer is at the kiosk
    And the payment will succeed
    When they order a "Classic Milk Tea" with default options
    And the kitchen staff marks the order as complete
    Then the order is no longer on the active list
