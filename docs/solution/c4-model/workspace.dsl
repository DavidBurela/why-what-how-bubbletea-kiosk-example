workspace {

    model {
        // Persons
        customer = person "Customer" "Orders customised bubble tea drinks via the self-service kiosk." "Customer"
        kitchenStaff = person "Kitchen Staff" "Prepares drinks using the kitchen display and calls out order numbers." "Kitchen Staff"

        // Software Systems
        kioskSystem = softwareSystem "Compile & Sip Kiosk System" "Self-service ordering, payment processing, and kitchen display for Compile & Sip." {
            kioskTui = container "Kiosk TUI" "Customer-facing self-service ordering terminal. Displays menu, takes customisation, initiates payment." ".NET Console App (Terminal UI)" "Application"
            orderApi = container "Order API" "Central backend. Receives orders, stores state in memory, calls payment gateway, serves orders to kitchen display." "ASP.NET Core Minimal API" "Application"
            kitchenDisplay = container "Kitchen Display" "Staff-facing display showing active orders with full customisation details. Staff mark orders as complete." ".NET Console App" "Application"
        }

        paymentGateway = softwareSystem "Payment Gateway" "Processes card payments (simulated external provider)." "Existing System"

        // Relationships
        customer -> kioskTui "Places orders and pays" "Terminal UI"
        kitchenStaff -> kitchenDisplay "Views active orders and marks them complete" "Terminal UI"
        kioskTui -> orderApi "Submits orders and initiates payment" "HTTP"
        orderApi -> paymentGateway "Processes card payments" "HTTP"
        kitchenDisplay -> orderApi "Polls for active orders and marks orders complete" "HTTP"
    }

    views {
        // System Context
        systemContext kioskSystem "SystemContext" {
            include *
            autoLayout
        }

        // Container view
        container kioskSystem "Containers" {
            include *
            autoLayout
        }

        // Dynamic view: FLW-KSK-001 — Customer Places Order
        dynamic kioskSystem "FLW-KSK-001-customer-places-order" "Customer browses menu, customises drink, and pays via the kiosk." {
            customer -> kioskTui "Browses menu and selects drink"
            customer -> kioskTui "Customises drink options"
            customer -> kioskTui "Reviews order and confirms"
            kioskTui -> orderApi "Submits order with customisations"
            orderApi -> paymentGateway "Processes card payment"
            orderApi -> kioskTui "Returns order number (or payment failure)"
            autoLayout
        }

        // Dynamic view: FLW-API-001 — Process Order and Payment
        dynamic kioskSystem "FLW-API-001-process-order-and-payment" "Order API receives order from kiosk, calls payment gateway, and stores result." {
            kioskTui -> orderApi "Sends order with drink and customisations"
            orderApi -> paymentGateway "Calls payment gateway to process card"
            paymentGateway -> orderApi "Returns payment result"
            orderApi -> kioskTui "Returns order number or payment failure"
            autoLayout
        }

        // Dynamic view: FLW-KDS-001 — Display and Complete Orders
        dynamic kioskSystem "FLW-KDS-001-display-and-complete-orders" "Kitchen Display polls for active orders and staff mark them complete." {
            kitchenDisplay -> orderApi "Polls for active orders"
            orderApi -> kitchenDisplay "Returns active orders with details"
            kitchenStaff -> kitchenDisplay "Marks order as complete"
            kitchenDisplay -> orderApi "Sends order completion update"
            autoLayout
        }

        styles {
            element "Person" {
                color #ffffff
                fontSize 22
                shape Person
            }
            element "Customer" {
                background #08427b
            }
            element "Kitchen Staff" {
                background #546e7a
            }
            element "Software System" {
                background #1168bd
                color #ffffff
            }
            element "Existing System" {
                background #999999
                color #ffffff
            }
        }

    }

}
