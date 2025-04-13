```mermaid
flowchart TD
    A((Start)) --> B[Input Parameters]
    B --> C[Create Request Body]
    C --> D[Add Authorization Header]
    D --> E{API Call Successful?}
    E -->|Yes| F[Parse Response]
    E -->|No| G[Return -1]
    F --> H{Valid Response?}
    H -->|Yes| I[Extract Probability]
    H -->|No| G
    I --> J[Return Probability]
    J --> K((End))
    G --> K

    subgraph Input Parameters
        B1{{daysBeforeService}}
        B2{{status}}
        B3{{totalAmount}}
    end

    subgraph API Configuration
        C1{{Model: meta-llama/Llama-Vision-Free}}
        C2{{System Message}}
        C3{{User Message with Parameters}}
        C4{{Temperature: 0.7}}
    end
