﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cubit32.Neurals
{
   ///<summary>
   ///every neuron's and connection's double value will be constrained by the sigmoid function<br/>
   ///the double values of neurons represent their base values (presigmoid)<br/>
   ///the double values of connections represent the weights they apply to their next neuron
   ///</summary>
   [Serializable]
   public class Brain
   {
      private int Perceptrons { get; set; }
      private int OutputNeurons { get; set; }
      private double[,] HiddenNeuronsBiasVals { get; set; }
      private double[] OutputNeuronsBiasValues { get; set; }
      public int HiddenLayerHeight { get; set; }
      public int HiddenLayerWidth { get; set; }
      // the first index represents connection layer (the fist involve perceptrons, the last 
      // involve output neurons and the others involve intra-hidden layer connections)
      private double[][] NeuronConnections { get; set; }
      /// <summary>Counts the number of neurons and connectors</summary>
      private int NeuronsPlusConnectionsCount {
         get
         {
            return HiddenNeuronsBiasVals.Length + OutputNeuronsBiasValues.Length+NeuronConnectionsCount;
         }
      }
      private int NeuronConnectionsCount {
         get
         {
            int count = 0;
            foreach (var neuronLayer in NeuronConnections)
            {
               count+=neuronLayer.Length;
            }
            return count;
         }
      }

      public Brain(
          int perceptrons,
          int hiddenlayerwidth,
          int hiddenlayerheight,
          int outputneurons)
      {
         Perceptrons = perceptrons;
         HiddenNeuronsBiasVals = new double[hiddenlayerwidth, hiddenlayerheight];
         OutputNeuronsBiasValues = new double[outputneurons];
         NeuronConnections = new double[hiddenlayerwidth + 1][];
         HiddenLayerHeight = hiddenlayerheight;
         HiddenLayerWidth = hiddenlayerwidth;
         OutputNeurons = outputneurons;

         NeuronConnections[0] = new double[perceptrons * hiddenlayerheight];
         for (int i = 1; i < NeuronConnections.Length - 1; i++)
            NeuronConnections[i] = new double[hiddenlayerheight * hiddenlayerheight];
         NeuronConnections[NeuronConnections.Length - 1] = new double[outputneurons * hiddenlayerheight];

         for (int j = 0; j < hiddenlayerwidth; j++)
            for (int i = 0; i < hiddenlayerheight; i++)
               HiddenNeuronsBiasVals[j, i] = RandomDouble();

         for (int i = 0; i < NeuronConnections.Length; i++)
            for (int j = 0; j < NeuronConnections[i].Length; j++)
               NeuronConnections[i][j] = RandomDouble();

         for (int i = 0; i < OutputNeuronsBiasValues.Length; i++)
            OutputNeuronsBiasValues[i] = RandomDouble();
      }

      public double[] InputToOutput(double[] perceptronValues)
      {
         if (perceptronValues.Length != Perceptrons)
            throw new Exception("perceptron values must be of equal length to the set number of perceptrons of this brain");

         // propagate the first time from perceptrons to hidden layer
         double[] nextLayerNeurons = new double[HiddenLayerHeight];
         for (int i = 0; i < HiddenLayerHeight; i++)
         {
            nextLayerNeurons[i] = 0;
            for (int j = 0; j < Perceptrons; j++)
               nextLayerNeurons[i] += perceptronValues[j] * NeuronConnections[0][j + i * Perceptrons];
            nextLayerNeurons[i] += HiddenNeuronsBiasVals[0, i];
            nextLayerNeurons[i] = Squash(nextLayerNeurons[i]);
         }

         // propagate values from one hidden layer to the next
         double[] currentLayerNeurons = new double[HiddenLayerHeight];
         for (int i = 1; i < HiddenLayerWidth; i++)
         {
            currentLayerNeurons = nextLayerNeurons;
            nextLayerNeurons = new double[HiddenLayerHeight];
            for (int j = 0; j < nextLayerNeurons.Length; j++)
               nextLayerNeurons[j] = 0;
            for (int j = 0; j < HiddenLayerHeight; j++)
            {
               for (int k = 0; k < HiddenLayerHeight; k++)
                  nextLayerNeurons[j] += currentLayerNeurons[i - 1] * NeuronConnections[i][j * HiddenLayerHeight + k];
               nextLayerNeurons[j] += HiddenNeuronsBiasVals[i - 1, j];
               nextLayerNeurons[j] = Squash(nextLayerNeurons[j]);
            }
         }

         // propagate from final hidden layer to output neurons
         currentLayerNeurons = nextLayerNeurons;
         nextLayerNeurons = new double[OutputNeurons];
         for (int i = 0; i < OutputNeurons; i++)
         {
            nextLayerNeurons[i] = 0;
            for (int j = 0; j < HiddenLayerHeight; j++)
               nextLayerNeurons[i] += NeuronConnections[NeuronConnections.Length - 1][i * OutputNeurons + j] * currentLayerNeurons[j];
            nextLayerNeurons[i] += OutputNeuronsBiasValues[i];
            nextLayerNeurons[i] = Squash(nextLayerNeurons[i]);
         }

         return nextLayerNeurons;
      }
      /// <summary>
      /// Randomly mutates the neurons and neuron connections of this AI to a degree specified by the arguments<br/><br/>
      /// </summary>
      /// <param name="degree">determines the amount by which a neuron can change.</param>
      /// <param name="normalRangeRepeats">normalRangeRepeats determines how much the random number approaches a normal distribution.</param>
      /// <param name="chanceOfMutation">Determines the numbber of mutations there will be on average</param>
      public void Mutate(double degree, int normalRangeRepeats = 4, int chanceOfMutation = 10)
      {   //todo make a method that instead of mutating deterministically instead varies each element of the brain and re-scores at every variation and saves when it       finds an improvement.
         chanceOfMutation = (
         Perceptrons * HiddenLayerHeight +
         HiddenLayerHeight * (HiddenLayerWidth - 1) +
         HiddenLayerHeight * OutputNeurons)
         / chanceOfMutation;

         Random rng = new Random((int)DateTime.Now.Ticks);
         for (int i = 0; i < HiddenLayerWidth; i++)
            for (int j = 0; j < HiddenLayerHeight; j++)
            {
               HiddenNeuronsBiasVals[i, j] += MutationMagnitude(normalRangeRepeats, degree) * Convert.ToInt32(rng.Next(chanceOfMutation) == 0);
            }
         for (int i = 0; i < OutputNeurons; i++)
         {
            OutputNeuronsBiasValues[i] += MutationMagnitude(normalRangeRepeats, degree) * Convert.ToInt32(rng.Next(chanceOfMutation) == 0);
         }
         for (int i = 0; i < NeuronConnections.Length; i++)
            for (int j = 0; j < NeuronConnections[i].Length; j++)
            {
               NeuronConnections[i][j] += MutationMagnitude(normalRangeRepeats, degree) * Convert.ToInt32(rng.Next(chanceOfMutation) == 0);
            }
      }
      public double RandomDouble(double range = 1)
      {
         Random rng = new Random();
         return (double)rng.Next(5) - 2;
      }

      public static double Squash(double value)
      {
         return Math.Max(0, value);
      }

      public double MutationMagnitude(int nrr, double degree)
      {
         double tempDouble = 0;
         for (int k = 0; k < nrr; k++)
            tempDouble += RandomDouble(degree);
         tempDouble /= nrr;
         return tempDouble;
      }

      /// <summary>
      /// Changes the value of a neuron connection or neuron base value. Starts with perceptron connectors and ends with output bias values. Finally it starts over depending no the 'increment' value.
      /// </summary>
      /// <param name="increment">An integer 0 or above. If 0 it changes the first neuron connection (from perceptron to middle layer neuron). Once increment it moves on to the next neuron or neuron connection from beginning to end starting at the perceptron connectors and finally returns (through a modulo function) to the perceptron connectors.</param>
      /// <param name="magnitude">By how much to change the target neuron</param>
      /// <param name="add">if false, multiplies the modified value with the magnitude, otherwise adds</param>
      public void MutateDeterministically(int increment, double magnitude, bool add)
      {
         if (increment < 0) throw new ArgumentException("increment may not be smaller than 0");
         magnitude = Math.Pow(magnitude, 10 - increment % (NeuronsPlusConnectionsCount * 10)/NeuronsPlusConnectionsCount);
         increment %= NeuronsPlusConnectionsCount;

         //change a perceptron connection value
         if (NeuronConnectionsCount < increment)
         {
            increment -= NeuronConnectionsCount;
         }
         else
         {
            //select layer
            int layerDepth = 0;
            while (NeuronConnections[layerDepth].Length < increment) increment -= NeuronConnections[layerDepth++].Length;

            //change value
            if (add) NeuronConnections[layerDepth][increment%NeuronConnections[layerDepth].Length] += magnitude;
            else NeuronConnections[layerDepth][increment%NeuronConnections[layerDepth].Length] *= magnitude;
            return;
         }

         //or an output neuron bias value
         if (OutputNeuronsBiasValues.Length < increment)
         {
            increment -= OutputNeuronsBiasValues.Length;
         }
         else
         {
            if (add) OutputNeuronsBiasValues[increment%OutputNeuronsBiasValues.Length] += magnitude;
            else OutputNeuronsBiasValues[increment%OutputNeuronsBiasValues.Length] *= magnitude;
            return;
         }
         
         //or change a hidden neuron bias value
         if (add) HiddenNeuronsBiasVals[increment%HiddenLayerWidth, increment%HiddenLayerHeight] += magnitude;
         else HiddenNeuronsBiasVals[increment%HiddenLayerWidth, increment%HiddenLayerHeight] *= magnitude;

      }
   }
}