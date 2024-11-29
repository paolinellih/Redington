import React from 'react';

export const ValidationErrorsDisplay: React.FC<{ errors?: Record<string, string[]> | null }> = ({ errors }) => {
    // Return an empty div or null if errors is null or undefined
    if (errors == null || !errors) {
        return null;
    }

    return (
        <div style={{marginTop: "20px", color: "red"}}>
            {Object.entries(errors).map(([field, errorMessages]) => (
                <div key={field}>
                    <strong>{field}:</strong>
                    <ul>
                        {errorMessages.map((error, index) => (
                            <li key={index}>{error}</li>
                        ))}
                    </ul>
                </div>
            ))}
        </div>
    );
};
