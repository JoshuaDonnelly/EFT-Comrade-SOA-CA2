namespace CA_2.DTOs;

// This DTO describes the response for the API
public record QuestProgressDto
(
    int QuestId,
    string QuestTitle,
    string Status,
    DateTime? CompletedAt
);