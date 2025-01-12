using ExpCurvFitting.Application.InputData;
using ExpCurvFitting.Application.TemplateHeadersGenerator;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Test;

public class InputDataMidRadTests
{

    [Fact]
    public void Test1()
    {
        // Arrange
        var configuration = new InputDataConfiguration()
        {
            CountInputVariable = 1,
            IntervalPresentation = IntervalPresentation.MidRad,
            IsIntervalInput = true,
            IsIntervalOutput = true
        };
        
        var data = new double[][]
        {
            [1.0, 2.0], // x_mid
            [0.5, 0.5], // x_rad
            [3.0, 4.0], // y_mid
            [0.7, 0.3] // y_rad
        };
        
        var inputData = new InputDataMidRad(data, configuration);
        
        // act & assert
        Assert.Equal(Vector<double>.Build.DenseOfArray([1.5, 2.5]), inputData.XUb[0]);
        Assert.Equal(Vector<double>.Build.DenseOfArray([0.5, 1.5]), inputData.XLb[0]);
        Assert.Equal(Vector<double>.Build.DenseOfArray([3.7, 4.3]), inputData.YUb);
        Assert.Equal(Vector<double>.Build.DenseOfArray([2.3, 3.7]), inputData.YLb);
    }
    
    [Fact]
    public void Test2()
    {
        // Arrange
        var configuration = new InputDataConfiguration()
        {
            CountInputVariable = 2,
            IntervalPresentation = IntervalPresentation.MidRad,
            IsIntervalInput = true,
            IsIntervalOutput = true
        };
        
        var data = new double[][]
        {
            [1.0, 2.0], // x_mid
            [0.5, 0.5], // x_rad
            [2.0, 3.0], // x_mid
            [0.5, 0.5], // x_rad
            [3.0, 4.0], // y_mid
            [0.7, 0.3] // y_rad
        };
        
        var inputData = new InputDataMidRad(data, configuration);
        
        // act & assert
        Assert.Equal(Vector<double>.Build.DenseOfArray([1.5, 2.5]), inputData.XUb[0]);
        Assert.Equal(Vector<double>.Build.DenseOfArray([0.5, 1.5]), inputData.XLb[0]);
        Assert.Equal(Vector<double>.Build.DenseOfArray([2.5, 3.5]), inputData.XUb[1]);
        Assert.Equal(Vector<double>.Build.DenseOfArray([1.5, 2.5]), inputData.XLb[1]);
        Assert.Equal(Vector<double>.Build.DenseOfArray([3.7, 4.3]), inputData.YUb);
        Assert.Equal(Vector<double>.Build.DenseOfArray([2.3, 3.7]), inputData.YLb);
    }
    
    [Fact]
    public void Test3()
    {
        // Arrange
        var configuration = new InputDataConfiguration()
        {
            CountInputVariable = 2,
            IntervalPresentation = IntervalPresentation.MidRad,
            IsIntervalInput = true,
            IsIntervalOutput = true
        };
        
        var data = new double[][]
        {
            [1.0, 2.0], // x_mid
            [0.5, 0.5], // x_rad
            [2.0], // x_mid -- poison data
            [0.5, 0.5], // x_rad
            [3.0, 4.0], // y_mid
            [0.7, 0.3] // y_rad
        };
        
        var action = () => new InputDataMidRad(data, configuration);
        
        // act & assert        
        action.Should().Throw<ArgumentException>().WithMessage("input data must have same length");
    }
}