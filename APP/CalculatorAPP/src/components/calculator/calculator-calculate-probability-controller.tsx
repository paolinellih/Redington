import { CalculateProbabilityRequest, CalculateProbabilityResponse, GetProbabilityTypeResponse } from "./calculate-probability-interfaces";
export const CalculatorCalculateProbabilityController = {
    calculateProbability: async (
        request: CalculateProbabilityRequest
    ): Promise<CalculateProbabilityResponse> => {
        

        const apiUrl = `${import.meta.env.VITE_CALCULATOR_API_URL}/api/Calculator/calculate-probability`;
        try {
            const response = await fetch(apiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                credentials: "include", // Include credentials for cross-origin requests
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.status} ${response.statusText}`);
            }

            const data = await response.json();
            return data;
        } catch (error : any) {
            console.error("Error calculating probability:", error);
            throw new Error(error.message || "Failed to calculating probability");
        }
    },

    getProbabilityTypes: async (): Promise<GetProbabilityTypeResponse> => {
        const apiUrl = `${import.meta.env.VITE_CALCULATOR_API_URL}/api/Calculator/probability-types`;
        try {
            const response = await fetch(apiUrl, {
                credentials: "include", // Include credentials for cross-origin requests
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.status} ${response.statusText}`);
            }

            const data = await response.json();
            return data;
        } catch (error : any) {
            console.error("Error fetching probability types:", error);
            throw new Error(error.message || "Failed to fetch probability types");
        }
    },
};