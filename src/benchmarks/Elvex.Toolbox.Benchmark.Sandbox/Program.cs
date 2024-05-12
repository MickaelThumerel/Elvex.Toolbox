// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

using BenchmarkDotNet.Running;

using Elvex.Toolbox.Benchmark.Sandbox;

BenchmarkRunner.Run<StringSplitOptiBenchmark>();

//var inst = new StringSplitOptiBenchmark();
//inst.ClassicSplit();
//inst.OptiSplit();
//inst.LessOptiSplit();
