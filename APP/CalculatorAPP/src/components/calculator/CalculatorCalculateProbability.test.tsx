import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import CalculatorCalculateProbability from './calculator-calculate-probability';
import { CalculatorCalculateProbabilityController } from './calculator-calculate-probability-controller';
import '@testing-library/jest-dom';

// Mock external module calls like API calls
jest.mock('./calculator-calculate-probability-controller', () => ({
    CalculatorCalculateProbabilityController: {
        getProbabilityTypes: jest.fn(),
        calculateProbability: jest.fn(),
    },
}));

describe('CalculatorCalculateProbability Component', () => {

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('shows validation error when input is invalid', async () => {
        render(<CalculatorCalculateProbability />);

        // Simulate invalid input
        const inputA = screen.getByLabelText(/Probability A/i);
        fireEvent.change(inputA, { target: { value: '2' } });

        // Wait for the validation message to appear
        const errorMessages = await screen.findAllByText(/Must be between 0 and 1/i);
        expect(errorMessages).toHaveLength(1);
    });

    test('shows validation error when input is invalid', async () => {
        render(<CalculatorCalculateProbability />);

        // Simulate invalid input
        const inputA = screen.getByLabelText(/Probability B/i);
        fireEvent.change(inputA, { target: { value: '2' } });

        // Wait for the validation message to appear
        const errorMessages = await screen.findAllByText(/Must be between 0 and 1/i);
        expect(errorMessages).toHaveLength(1);  // Expecting two error messages, one for each field
    });

    test('renders the component correctly', async () => {
        render(<CalculatorCalculateProbability />);

        // Wait for the button text to change
        const button = await screen.findByRole('button', { name: /Calculate/i });
        expect(button).toBeInTheDocument();
    });

    describe('CalculatorCalculateProbability Component', () => {
        const getProbabilityTypes = CalculatorCalculateProbabilityController.getProbabilityTypes as jest.Mock;
        const calculateProbability = CalculatorCalculateProbabilityController.calculateProbability as jest.Mock;

        beforeEach(() => {
            getProbabilityTypes.mockResolvedValue({
                response: {
                    successful: true,
                    result: [
                        { value: 'Select a type', name: 'Select a type' },
                        { value: 'Either', name: 'Either' },
                        { value: 'Combined With', name: 'Combined With' },
                    ]
                }
            });
            
            calculateProbability.mockResolvedValue({
                response: {
                    successful: true,
                    result: 0.75,
                }
            });
        });

        afterEach(() => {
            jest.clearAllMocks();
        });

        it('submitting the form with valid data calls the calculateProbability API', async () => {
            render(<CalculatorCalculateProbability />);

            // Wait for the dropdown to be populated with options (other than the default one)
            const selectElement = await screen.findByLabelText(/Probability Type/i);

            // Ensure options other than "Select a type" are loaded
            await waitFor(() => {
                expect(selectElement.children.length).toBeGreaterThan(1);
            });

            // Fill in the form fields
            fireEvent.change(screen.getByLabelText(/Probability A/i), { target: { value: '0.5' } });
            fireEvent.change(screen.getByLabelText(/Probability B/i), { target: { value: '0.5' } });

            // Select 'Combined With' from the dropdown
            fireEvent.change(selectElement, { target: { value: 'Combined With' } });

            // Find the form element and submit it manually
            const form = screen.getByRole('form');  // Ensure the form is present
            fireEvent.submit(form);  // Manually trigger form submission

            // Wait for the calculateProbability API to be called
            await waitFor(() => {
                expect(calculateProbability).toHaveBeenCalledWith({
                    a: "0.5",
                    b: "0.5",
                    type: "Combined With",
                });
            });

            // Log the mock calls to debug
            console.log("calculateProbability mock calls: ", calculateProbability.mock.calls);
        });

        it('displays generic error when API request fails', async () => {
            CalculatorCalculateProbabilityController.calculateProbability = jest.fn(() =>
                Promise.reject(new Error("An error occurred while calculating probability."))
            );

            render(<CalculatorCalculateProbability />);

            // Wait for the dropdown to be populated with options (other than the default one)
            const selectElement = await screen.findByLabelText(/Probability Type/i);

            // Ensure options other than "Select a type" are loaded
            await waitFor(() => {
                expect(selectElement.children.length).toBeGreaterThan(1);
            });

            // Fill in the form fields
            fireEvent.change(screen.getByLabelText(/Probability A/i), { target: { value: '0.5' } });
            fireEvent.change(screen.getByLabelText(/Probability B/i), { target: { value: '0.5' } });

            // Select 'Combined With' from the dropdown
            fireEvent.change(selectElement, { target: { value: 'Combined With' } });

            // Wait for the button text to change
            const calculateButton = await screen.findByRole('button', { name: /Calculate/i });

            // Attempt to submit without selecting a type
            fireEvent.click(calculateButton);

            await waitFor(() => {
                expect(
                    screen.getByText(/An error occurred while calculating probability/i)
                ).toBeInTheDocument();
            });
        });
    });

    describe('CalculatorCalculateProbability - Validation Errors', () => {
        it('sets validation errors when the API response contains validation errors', async () => {
            // Mock the calculateProbability function to return validation errors
            CalculatorCalculateProbabilityController.calculateProbability = jest.fn().mockResolvedValue({
                response: {
                    successful: false,
                    validationErrors: {
                        a: ['Must be a number between 0 and 1.'],
                        b: ['This field is required.'],
                        type: ['Please select a valid probability type.'],
                    },
                },
            });

            render(<CalculatorCalculateProbability/>);

            // Wait for the dropdown to be populated with options (other than the default one)
            const selectElement = await screen.findByLabelText(/Probability Type/i);

            // Ensure options other than "Select a type" are loaded
            await waitFor(() => {
                expect(selectElement.children.length).toBeGreaterThan(1);
            });

            // Fill in the form fields
            fireEvent.change(screen.getByLabelText(/Probability A/i), { target: { value: '0.5' } });
            fireEvent.change(screen.getByLabelText(/Probability B/i), { target: { value: '0.5' } });

            // Select 'Combined With' from the dropdown
            fireEvent.change(selectElement, { target: { value: 'Combined With' } });

            // Wait for the button text to change
            const calculateButton = await screen.findByRole('button', { name: /Calculate/i });

            // Attempt to submit without selecting a type
            fireEvent.click(calculateButton);

            // Wait for the API call to resolve
            await waitFor(() => {
                // Check if validation errors are displayed
                expect(screen.getByText(/Must be a number between 0 and 1./i)).toBeInTheDocument();
                expect(screen.getByText(/This field is required./i)).toBeInTheDocument();
                expect(screen.getByText(/Please select a valid probability type./i)).toBeInTheDocument();
            });
        });
    });

    test('handles error when fetching probability types fails', async () => {
        // Mock the function to reject with an error
        CalculatorCalculateProbabilityController.getProbabilityTypes = jest.fn().mockRejectedValueOnce(
            new Error('Failed to fetch probability types.')
        );

        render(<CalculatorCalculateProbability />);

        // Wait for the error message to appear
        await waitFor(() =>
            expect(screen.getByText('Failed to fetch probability types.')).toBeInTheDocument()
        );
    });

    test('shows validation error for Probability Type field', async () => {
        render(<CalculatorCalculateProbability />);

        // Wait for the button text to change
        const calculateButton = await screen.findByRole('button', { name: /Calculate/i });
        
        // Attempt to submit without selecting a type
        fireEvent.click(calculateButton);

        expect(
            screen.getByText(/Please select a Probability Type/i)
        ).toBeInTheDocument();
    });
    
    test('sets isValid to false if a field is invalid', async () => {
        render(<CalculatorCalculateProbability />);

        const probabilityAInput = screen.getByLabelText(/Probability A/i);
        fireEvent.change(probabilityAInput, { target: { value: "-1" } });

        // Wait for the button text to change
        const calculateButton = await screen.findByRole('button', { name: /Calculate/i });

        // Attempt to submit without selecting a type
        fireEvent.click(calculateButton);

        expect(
            screen.getByText(/Probability A Must be between 0 and 1/i)
        ).toBeInTheDocument();
    });

    test('sets isValid to false if b field is invalid', async () => {
        render(<CalculatorCalculateProbability />);

        const probabilityAInput = screen.getByLabelText(/Probability B/i);
        fireEvent.change(probabilityAInput, { target: { value: "-1" } });

        // Wait for the button text to change
        const calculateButton = await screen.findByRole('button', { name: /Calculate/i });

        // Attempt to submit without selecting a type
        fireEvent.click(calculateButton);

        expect(
            screen.getByText(/Probability B Must be between 0 and 1/i)
        ).toBeInTheDocument();
    });
});