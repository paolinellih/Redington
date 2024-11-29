// Calculate probability
export interface CalculateProbabilityRequest {
    a: number|null;
    b: number|null;
    type: string;
}
export interface CalculateProbabilityRequestError {
    a: string;
    b: string;
    type: string;
}

export interface CalculateProbabilityResponse {
    response: {
        result: number;
        successful: boolean;
        validationErrors : Record<string, string[]>;
    };
}

// Probability types
export interface ProbabilityTypeDto {
    value: string;
    name: string;
}
export interface GetProbabilityTypeResponse {
    response: {
        successful: boolean;
        result: ProbabilityTypeDto[];
        validationErrors : Record<string, string[]>;
    };
}