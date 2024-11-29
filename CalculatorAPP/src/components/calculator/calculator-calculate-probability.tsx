import React, {useEffect, useState} from "react";
import './calculator-calculate-probability.css';
import {CalculatorCalculateProbabilityController} from "./calculator-calculate-probability-controller";
import {
    CalculateProbabilityRequest,
    CalculateProbabilityRequestError,
    CalculateProbabilityResponse,
    ProbabilityTypeDto
} from "./calculate-probability-interfaces";
import {ValidationErrorsDisplay} from "../ValidationErrorsDisplay.tsx";

const CalculatorCalculateProbability: React.FC = () => {
    const [type, setType] = useState<number>(-1);
    const [types, setTypes] = useState<ProbabilityTypeDto[]>([]); // Available probability types
    const [result, setResult] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [fetching, setFetching] = useState<boolean>(false);
    const [action, setAction] = useState<string>("Loading");
    const [logoLoaded, setLogoLoaded] = useState(false);
    const [validationErrors, setValidationErrors] = useState<Record<string, string[]> | null>(null);

    // Trigger the logo animation on component mount
    useEffect(() => {
        setLogoLoaded(true);
    }, []);

    // State to store the request data
    const [request, setRequest] = useState<CalculateProbabilityRequest>({
        a: null, 
        b: null, 
        type: '',
    });

    // Track the initial values to compare later
    const [initialRequest, setInitialRequest] = useState<CalculateProbabilityRequest>({
        a: null,
        b: null,
        type: '',
    });

    // State to store validation errors (initialized with empty strings)
    const [errors, setErrors] = useState<CalculateProbabilityRequestError>({
        a: '',
        b: '',
        type: '',
    });


    // Fetch probability types when the component loads
    useEffect(() => {
        const fetchTypes = async () => {
            setFetching(true);
            try {
                const data = await CalculatorCalculateProbabilityController.getProbabilityTypes();
                if (data?.response.successful) {
                    setTypes(data?.response.result || []);
                } else {
                    setValidationErrors(data?.response?.validationErrors); // Validation Errors
                }
                setFetching(false);
            } catch (error : any) {
                setError(error.message || "Failed to fetch probability types.");
            } finally {
                setFetching(false);
            }
        };

        fetchTypes();
    }, []);

    // Set the initial state when the component is mounted
    useEffect(() => {
        setInitialRequest(request); // Initialize the request state
    }, []);

    // Validation for each field
    const validateField = (field: keyof CalculateProbabilityRequest, value: any) => {
        let error = '';

        if (field === 'a' || field === 'b') {
            const numValue = parseFloat(value);
            if (isNaN(numValue) || numValue < 0 || numValue > 1) {
                error = 'Must be between 0 and 1.';
            }
        }

        if (field === 'type' && (!value || value === '-1')) {
            error = 'Please select a Probability Type.';
        }

        setErrors((prevErrors) => ({
            ...prevErrors,
            [field]: error,
        }));

        return error === ''; // Return true if there is no error
    };

    // Handle input changes
    const handleInputChange = (field: keyof typeof request) =>
        (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {

            const value = e.target.value;

            // Update the request state
            setRequest({
                ...request,
                [field]: value
            });

            // If the value is different from the initial state, validate it
            if (value !== initialRequest[field]) {
                validateField(field, value);
            }
        };

    // Validate all fields together
    const validateAllFields = (): boolean => {
        const fields: (keyof CalculateProbabilityRequest)[] = ['a', 'b', 'type'];
        let isValid = true;

        fields.forEach((field) => {
            const fieldValid = validateField(field, request[field]);
            if (!fieldValid) {
                isValid = false; // If any field is invalid, set isValid to false
            }
        });

        return isValid;
    };

    // Handle form submission
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        const isValid = validateAllFields();
        
        if (isValid) {
            setFetching(true);
            setAction("Calculating");
            setError(null);
            setResult(null);
            setValidationErrors(null);
            try {
                const data: CalculateProbabilityResponse =
                    await CalculatorCalculateProbabilityController.calculateProbability(
                        request
                    );
                if (data?.response?.successful) {
                    setResult(data.response.result.toString());
                } else {
                    setValidationErrors(data?.response?.validationErrors); // Validation Errors
                }
            } catch (error: any) {
                setError(error.message || "An error occurred while calculating probability.");
            } finally {
                setFetching(false);
            }
        }
    };

    return (
        <div style={{padding: "20px", maxWidth: "400px", margin: "0 auto"}}>
            <a href="https://redington.co.uk/" target="_blank"><img
                id="redingtonLogo"
                className={logoLoaded ? "logo-animation" : ""} // Apply animation when loaded
                src="https://redington.co.uk/wp-content/uploads/2020/11/redington-logo-dark.png"
                alt="Redington Logo"
            /></a>
            <h1>Probability Calc</h1>
            <form onSubmit={handleSubmit}>
                <div className="form-floating mb-3">
                    <input
                        id="probabilityA"
                        className={`form-control ${errors.a ? 'is-invalid' : ''}`} // Add invalid class if error exists
                        type="number"
                        step="0.1"
                        value={request.a ?? ""}
                        placeholder="Number between 0 and 1"
                        title="Number between 0 and 1"
                        onChange={handleInputChange("a")}
                    />
                    <label htmlFor="probabilityA">Probability A</label>
                    {errors.a && <div className="invalid-feedback">{errors.a}</div>} {/* Validation message */}
                </div>

                <div className="form-floating mb-3">
                    <input
                        id="probabilityB"
                        className={`form-control ${errors.b ? 'is-invalid' : ''}`} // Add invalid class if error exists
                        type="number"
                        step="0.1"
                        value={request.b ?? ""}
                        placeholder="Number between 0 and 1"
                        title="Number between 0 and 1"
                        onChange={handleInputChange("b")}
                    />
                    <label htmlFor="probabilityB">Probability B</label>
                    {errors.b && <div className="invalid-feedback">{errors.b}</div>} {/* Validation message */}
                </div>

                <div className="form-floating mb-3">
                    <select
                        id="probabilityType"
                        className={`form-control ${errors.type ? 'is-invalid' : ''}`} // Add invalid class if error exists
                        value={type}
                        title="Choose a probability type"
                        onChange={(e) => {
                            const selectedType = Number(e.target.value); // Get the selected value as a number
                            setType(selectedType); // Update the local state (type)
                            setRequest({
                                ...request,
                                type: selectedType.toString(), // Update the request object
                            });
                            validateField('type', selectedType.toString()); // Validate immediately
                        }}
                    >
                        <option value="-1" disabled>
                            Select a type
                        </option>
                        {Array.isArray(types) &&
                            types.map((typeOption) => (
                                <option key={typeOption.value} value={typeOption.value}>
                                    {typeOption.name}
                                </option>
                            ))}
                    </select>
                    <label htmlFor="probabilityType">Probability Type</label>
                    {errors.type && <div className="invalid-feedback">{errors.type}</div>} {/* Validation message */}
                </div>

                <button type="submit" className="btn redington-btn" disabled={fetching}>
                    {fetching ? <div>{action}...<span className="spinner-border spinner-border-sm ms-2" role="status"
                                                      aria-hidden="true"></span></div> : <span>Calculate</span>}
                </button>
            </form>
            {result != null && (
                <div style={{marginTop: "20px", color: "green"}}>
                    <p>Result:</p>
                    <h3>{result}</h3>
                </div>
            )}
            {error != null && (
                <div style={{marginTop: "20px", color: "red"}}>
                    <strong>{error}</strong>
                </div>
            )}
            <ValidationErrorsDisplay errors={validationErrors}></ValidationErrorsDisplay>
        </div>
    );
};

export default CalculatorCalculateProbability;
