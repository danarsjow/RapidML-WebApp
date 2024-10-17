import sys
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.linear_model import LinearRegression
import os

def main(file_path):
    # Load the data from the Excel file
    df = pd.read_excel(file_path)
    
    # Assume the first column is X and the second is Y
    X = df.iloc[:, 0].values.reshape(-1, 1)  # Reshape for sklearn
    y = df.iloc[:, 1].values
    
    # Create a linear regression model
    model = LinearRegression()
    model.fit(X, y)
    
    # Generate predictions
    y_pred = model.predict(X)
    
    # Plot
    plt.scatter(X, y, color='blue')
    plt.plot(X, y_pred, color='red')
    plt.title('Linear Regression')
    plt.xlabel('X')
    plt.ylabel('Y')

    slope = model.coef_[0]
    intercept = model.intercept_

    # Display the linear regression equation
    eq = f"y = {slope:.2f}x + {intercept:.2f}"
    plt.text(0.5, 0.9, eq, ha='center', va='center', transform=plt.gca().transAxes)
    
    # Save the plot
    output_path = os.path.join(os.path.dirname(file_path), 'plot.png')
    plt.savefig(output_path)
    plt.show()

if __name__ == "__main__":
    main(sys.argv[1])