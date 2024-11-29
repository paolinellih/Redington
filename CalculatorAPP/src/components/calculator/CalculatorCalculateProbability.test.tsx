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
        expect(errorMessages).toHaveLength(1);  // Expecting two error messages, one for each field
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
                Response: {
                    Successful: true,
                    Result: [
                        { Value: 1, Name: 'Type 1' },
                        { Value: 2, Name: 'Type 2' },
                    ]
                }
            });
            calculateProbability.mockResolvedValue({
                Response: {
                    Successful: true,
                    Result: 0.75,
                    ValidationErrors: {},
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

            // Wait until there are more than one option (since "Select a type" is the default)
            await waitFor(() => {
                expect(selectElement.children.length).toBeGreaterThan(1); // Ensure options other than "Select a type" are loaded
            });

            // Fill in the form fields
            fireEvent.change(screen.getByLabelText(/Probability A/i), { target: { value: '0.5' } });
            fireEvent.change(screen.getByLabelText(/Probability B/i), { target: { value: '0.5' } });
            fireEvent.change(selectElement, { target: { value: '1' } });

            // Click the Calculate button
            const calculateButton = await screen.findByText(/Calculate/i);
            await waitFor(() => expect(calculateButton).not.toBeDisabled());
            fireEvent.click(calculateButton);

            // Verify that the calculateProbability API was called with the correct values
            expect(CalculatorCalculateProbabilityController.calculateProbability).toHaveBeenCalledWith({
                a: "0.5",
                b: "0.5",
                type: "1",
            });
        });
    });
});