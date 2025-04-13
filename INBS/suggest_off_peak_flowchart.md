```mermaid
flowchart TD
    A((Start)) --> B{{bookingId}}
    B --> C[Begin Transaction]
    C --> D[Query Booking with ArtistStore and Store]
    D --> E{Booking Exists?}
    E -->|No| F[Throw Exception]
    E -->|Yes| G[Create SuggestBooking DTO]
    G --> H[Commit Transaction]
    H --> I[Call SuggestOffPeakTimeFromAI]
    I --> J{AI Response Valid?}
    J -->|Yes| K[Return Suggested Time]
    J -->|No| L[Return -1]
    K --> M((End))
    L --> M
    F --> N[Rollback Transaction]
    N --> O[Throw Exception]
    O --> M

    subgraph SuggestBooking DTO
        G1{{StoreName}}
        G2{{WorkingDate}}
        G3{{StartTime}}
        G4{{EndTime}}
        G5{{BreakTime}}
        G6{{TotalAmount}}
    end

    subgraph AI Request
        I1{{Model: meta-llama/Llama-Vision-Free}}
        I2{{System Message}}
        I3{{User Message with Parameters}}
        I4{{Temperature: 0.7}}
    end