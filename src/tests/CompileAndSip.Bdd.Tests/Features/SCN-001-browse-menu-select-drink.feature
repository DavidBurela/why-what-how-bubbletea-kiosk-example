Feature: Browse menu and select drink

  As a customer at the kiosk
  I want to see the available drinks
  So that I can choose what to order

  Scenario: Customer views the menu
    Given the customer is at the kiosk
    When they view the menu
    Then they see 3 drinks available

  Scenario: Each drink shows name, description and base price
    Given the customer is at the kiosk
    When they view the menu
    Then "Classic Milk Tea" is available at $5.50
    And "Taro Milk Tea" is available at $6.00
    And "Mango Green Tea" is available at $6.00

  Scenario: Each drink has a description
    Given the customer is at the kiosk
    When they view the menu
    Then "Classic Milk Tea" is described as "Traditional black tea with milk and tapioca pearls. The original."
    And "Taro Milk Tea" is described as "Creamy taro root blended with milk tea. Rich and slightly sweet."
    And "Mango Green Tea" is described as "Fresh mango with jasmine green tea. Light and refreshing."
