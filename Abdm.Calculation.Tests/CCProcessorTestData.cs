using Abdm.Calculation.Models;
using System.Numerics;

namespace Abdm.Calculation.Tests
{
    public static class CCProcessorTestData
    {
        public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] {
                new CCRequestMessage()
                {
                    IssoId = 38000331,
                    CPNumber = 7,
                    NagruzkaId = 40,
                    Snip = SnipEnum.sn62,
                    Direction = DriveDirection.Bidirection,
                    NagruzkaSchema = new NagruzkaSchema
                    {
                        Id = NagruzkaEnum.N11,
                        Type = NagruzkaTypeEnum.Single,
                        TypeName = "Колесная одиночная",
                        NameShort = "Н11 (НК-80)",
                        Width = 3.5f,
                        Length = 4.4f,
                        Distance = 0,
                        Axles = new Axle[]
                        {
                            new Axle {
                                Y = 0.4f,
                                Wx = 0.8f,
                                Wy = 0.2f,
                                Weight = 20.0f,
                                AbsY = 0.4f,
                                Wheels = [
                                    2.7f
                                ]
                            },
                            new Axle {
                                Y = 1.2f,
                                Wx = 0.8f,
                                Wy = 0.2f,
                                Weight = 20.0f,
                                AbsY = 1.6f,
                                Wheels = [
                                    2.7f
                                ]
                            },
                            new Axle {
                                Y = 1.2f,
                                Wx = 0.8f,
                                Wy = 0.2f,
                                Weight = 20.0f,
                                AbsY = 2.8f,
                                Wheels = [
                                    2.7f
                                ]
                            },
                            new Axle {
                                Y = 1.2f,
                                Wx = 0.8f,
                                Wy = 0.2f,
                                Weight = 20.0f,
                                AbsY = 4.0f,
                                Wheels = [
                                    2.7f
                                ]
                            }
                        },
                    },
                    Surface = new Surface
                    {
                        SurfacePoints = new System.Numerics.Vector3[]
                        {
                                         new Vector3{
                X = 0.0f,
                Y = 0.0f,
                Z = -0.030388f
            },
            new Vector3{
                X = 0.0f,
                Y = 13.7f,
                Z = -0.030135f
            },
            new Vector3{
                X = 1.0f,
                Y = 0.0f,
                Z = 0.0f
            },
            new Vector3{
                X = 1.0f,
                Y = 13.7f,
                Z = 0.0f
            },
            new Vector3{
                X = 2.67f,
                Y = 0.0f,
                Z = 0.0f
            },
            new Vector3{
                X = 2.67f,
                Y = 13.7f,
                Z = 0.0f
            },
            new Vector3{
                X = 4.34f,
                Y = 0.0f,
                Z = 0.0f
            },
            new Vector3{
                X = 4.34f,
                Y = 13.7f,
                Z = 0.0f
            },
            new Vector3{
                X = 6.01f,
                Y = 0.0f,
                Z = 0.0f
            },
            new Vector3{
                X = 6.01f,
                Y = 13.7f,
                Z = 0.0f
            },
            new Vector3{
                X = 7.68f,
                Y = 0.0f,
                Z = 0.0f
            },
            new Vector3{
                X = 7.68f,
                Y = 13.7f,
                Z = 0.0f
            },
            new Vector3{
                X = 9.35f,
                Y = 0.0f,
                Z = 0.0f
            },
            new Vector3{
                X = 9.35f,
                Y = 13.7f,
                Z = 0.0f
            },
            new Vector3{
                X = 10.35f,
                Y = 0.0f,
                Z = 0.00052f
            },
            new Vector3{
                X = 10.35f,
                Y = 13.7f,
                Z = 0.000515f
            },
            new Vector3{
                X = 0.0f,
                Y = 12.33f,
                Z = -0.046336f
            },
            new Vector3{
                X = 0.0f,
                Y = 10.96f,
                Z = -0.067137f
            },
            new Vector3{
                X = 0.0f,
                Y = 9.59f,
                Z = -0.087891f
            },
            new Vector3{
                X = 0.0f,
                Y = 8.22f,
                Z = -0.100536f
            },
            new Vector3{
                X = 0.0f,
                Y = 6.85f,
                Z = -0.102986f
            },
            new Vector3{
                X = 0.0f,
                Y = 5.48f,
                Z = -0.100694f
            },
            new Vector3{
                X = 0.0f,
                Y = 4.11f,
                Z = -0.088327f
            },
            new Vector3{
                X = 0.0f,
                Y = 2.74f,
                Z = -0.06774f
            },
            new Vector3{
                X = 0.0f,
                Y = 1.37f,
                Z = -0.046776f
            },
            new Vector3{
                X = 1.0f,
                Y = 12.33f,
                Z = -0.011463f
            },
            new Vector3{
                X = 1.0f,
                Y = 10.96f,
                Z = -0.023267f
            },
            new Vector3{
                X = 1.0f,
                Y = 9.59f,
                Z = -0.036576f
            },
            new Vector3{
                X = 1.0f,
                Y = 8.22f,
                Z = -0.05135f
            },
            new Vector3{
                X = 1.0f,
                Y = 6.85f,
                Z = -0.061375f
            },
            new Vector3{
                X = 1.0f,
                Y = 5.48f,
                Z = -0.051674f
            },
            new Vector3{
                X = 1.0f,
                Y = 4.11f,
                Z = -0.036893f
            },
            new Vector3{
                X = 1.0f,
                Y = 2.74f,
                Z = -0.023513f
            },
            new Vector3{
                X = 1.0f,
                Y = 1.37f,
                Z = -0.011614f
            },
            new Vector3{
                X = 10.35f,
                Y = 12.33f,
                Z = -0.000455f
            },
            new Vector3{
                X = 10.35f,
                Y = 10.96f,
                Z = -0.001266f
            },
            new Vector3{
                X = 10.35f,
                Y = 9.59f,
                Z = -0.001883f
            },
            new Vector3{
                X = 10.35f,
                Y = 8.22f,
                Z = -0.002274f
            },
            new Vector3{
                X = 10.35f,
                Y = 6.85f,
                Z = -0.002407f
            },
            new Vector3{
                X = 10.35f,
                Y = 5.48f,
                Z = -0.002271f
            },
            new Vector3{
                X = 10.35f,
                Y = 4.11f,
                Z = -0.001876f
            },
            new Vector3{
                X = 10.35f,
                Y = 2.74f,
                Z = -0.001257f
            },
            new Vector3{
                X = 10.35f,
                Y = 1.37f,
                Z = -0.000447f
            },
            new Vector3{
                X = 9.35f,
                Y = 12.33f,
                Z = -0.001066f
            },
            new Vector3{
                X = 9.35f,
                Y = 10.96f,
                Z = -0.002047f
            },
            new Vector3{
                X = 9.35f,
                Y = 9.59f,
                Z = -0.002838f
            },
            new Vector3{
                X = 9.35f,
                Y = 8.22f,
                Z = -0.003351f
            },
            new Vector3{
                X = 9.35f,
                Y = 6.85f,
                Z = -0.003528f
            },
            new Vector3{
                X = 9.35f,
                Y = 5.48f,
                Z = -0.00335f
            },
            new Vector3{
                X = 9.35f,
                Y = 4.11f,
                Z = -0.002836f
            },
            new Vector3{
                X = 9.35f,
                Y = 2.74f,
                Z = -0.002045f
            },
            new Vector3{
                X = 9.35f,
                Y = 1.37f,
                Z = -0.001064f
            },
            new Vector3{
                X = 2.67f,
                Y = 12.33f,
                Z = 0.01251f
            },
            new Vector3{
                X = 2.67f,
                Y = 10.96f,
                Z = 0.026244f
            },
            new Vector3{
                X = 2.67f,
                Y = 9.59f,
                Z = 0.042933f
            },
            new Vector3{
                X = 2.67f,
                Y = 8.22f,
                Z = 0.061878f
            },
            new Vector3{
                X = 2.67f,
                Y = 6.85f,
                Z = 0.08333f
            },
            new Vector3{
                X = 2.67f,
                Y = 5.48f,
                Z = 0.062064f
            },
            new Vector3{
                X = 2.67f,
                Y = 4.11f,
                Z = 0.043201f
            },
            new Vector3{
                X = 2.67f,
                Y = 2.74f,
                Z = 0.026475f
            },
            new Vector3{
                X = 2.67f,
                Y = 1.37f,
                Z = 0.012666f
            },
            new Vector3{
                X = 4.34f,
                Y = 12.33f,
                Z = 0.004396f
            },
            new Vector3{
                X = 4.34f,
                Y = 10.96f,
                Z = 0.007686f
            },
            new Vector3{
                X = 4.34f,
                Y = 9.59f,
                Z = 0.008989f
            },
            new Vector3{
                X = 4.34f,
                Y = 8.22f,
                Z = 0.008307f
            },
            new Vector3{
                X = 4.34f,
                Y = 6.85f,
                Z = 0.007381f
            },
            new Vector3{
                X = 4.34f,
                Y = 5.48f,
                Z = 0.008232f
            },
            new Vector3{
                X = 4.34f,
                Y = 4.11f,
                Z = 0.008915f
            },
            new Vector3{
                X = 4.34f,
                Y = 2.74f,
                Z = 0.007638f
            },
            new Vector3{
                X = 4.34f,
                Y = 1.37f,
                Z = 0.004373f
            },
            new Vector3{
                X = 6.01f,
                Y = 12.33f,
                Z = -0.001462f
            },
            new Vector3{
                X = 6.01f,
                Y = 10.96f,
                Z = -0.002747f
            },
            new Vector3{
                X = 6.01f,
                Y = 9.59f,
                Z = -0.003786f
            },
            new Vector3{
                X = 6.01f,
                Y = 8.22f,
                Z = -0.004477f
            },
            new Vector3{
                X = 6.01f,
                Y = 6.85f,
                Z = -0.004724f
            },
            new Vector3{
                X = 6.01f,
                Y = 5.48f,
                Z = -0.004496f
            },
            new Vector3{
                X = 6.01f,
                Y = 4.11f,
                Z = -0.00382f
            },
            new Vector3{
                X = 6.01f,
                Y = 2.74f,
                Z = -0.002783f
            },
            new Vector3{
                X = 6.01f,
                Y = 1.37f,
                Z = -0.001488f
            },
            new Vector3{
                X = 7.68f,
                Y = 12.33f,
                Z = -0.00174f
            },
            new Vector3{
                X = 7.68f,
                Y = 10.96f,
                Z = -0.003286f
            },
            new Vector3{
                X = 7.68f,
                Y = 9.59f,
                Z = -0.004493f
            },
            new Vector3{
                X = 7.68f,
                Y = 8.22f,
                Z = -0.005259f
            },
            new Vector3{
                X = 7.68f,
                Y = 6.85f,
                Z = -0.005523f
            },
            new Vector3{
                X = 7.68f,
                Y = 5.48f,
                Z = -0.005264f
            },
            new Vector3{
                X = 7.68f,
                Y = 4.11f,
                Z = -0.004501f
            },
            new Vector3{
                X = 7.68f,
                Y = 2.74f,
                Z = -0.003294f
            },
            new Vector3{
                X = 7.68f,
                Y = 1.37f,
                Z = -0.001746f
            },
            new Vector3{
                X = 0.91f,
                Y = 0.0f,
                Z = -0.002552f
            },
            new Vector3{
                X = 0.91f,
                Y = 13.7f,
                Z = -0.002524f
            },
            new Vector3{
                X = 2.58f,
                Y = 0.0f,
                Z = 0.000397f
            },
            new Vector3{
                X = 2.58f,
                Y = 13.7f,
                Z = 0.000348f
            },
            new Vector3{
                X = 4.25f,
                Y = 0.0f,
                Z = 0.000384f
            },
            new Vector3{
                X = 4.25f,
                Y = 13.7f,
                Z = 0.000372f
            },
            new Vector3{
                X = 5.92f,
                Y = 0.0f,
                Z = -1.6e-05f
            },
            new Vector3{
                X = 5.92f,
                Y = 13.7f,
                Z = -7e-06f
            },
            new Vector3{
                X = 7.59f,
                Y = 0.0f,
                Z = -2.6e-05f
            },
            new Vector3{
                X = 7.59f,
                Y = 13.7f,
                Z = -2.3e-05f
            },
            new Vector3{
                X = 9.26f,
                Y = 0.0f,
                Z = -3.6e-05f
            },
            new Vector3{
                X = 9.26f,
                Y = 13.7f,
                Z = -3.6e-05f
            },
            new Vector3{
                X = 0.91f,
                Y = 12.33f,
                Z = -0.014474f
            },
            new Vector3{
                X = 0.91f,
                Y = 10.96f,
                Z = -0.027239f
            },
            new Vector3{
                X = 0.91f,
                Y = 9.59f,
                Z = -0.041432f
            },
            new Vector3{
                X = 0.91f,
                Y = 8.22f,
                Z = -0.055991f
            },
            new Vector3{
                X = 0.91f,
                Y = 6.85f,
                Z = -0.064577f
            },
            new Vector3{
                X = 0.91f,
                Y = 5.48f,
                Z = -0.056289f
            },
            new Vector3{
                X = 0.91f,
                Y = 4.11f,
                Z = -0.041763f
            },
            new Vector3{
                X = 0.91f,
                Y = 2.74f,
                Z = -0.027518f
            },
            new Vector3{
                X = 0.91f,
                Y = 1.37f,
                Z = -0.014662f
            },
            new Vector3{
                X = 9.26f,
                Y = 12.33f,
                Z = -0.001118f
            },
            new Vector3{
                X = 9.26f,
                Y = 10.96f,
                Z = -0.002119f
            },
            new Vector3{
                X = 9.26f,
                Y = 9.59f,
                Z = -0.002928f
            },
            new Vector3{
                X = 9.26f,
                Y = 8.22f,
                Z = -0.003453f
            },
            new Vector3{
                X = 9.26f,
                Y = 6.85f,
                Z = -0.003635f
            },
            new Vector3{
                X = 9.26f,
                Y = 5.48f,
                Z = -0.003452f
            },
            new Vector3{
                X = 9.26f,
                Y = 4.11f,
                Z = -0.002926f
            },
            new Vector3{
                X = 9.26f,
                Y = 2.74f,
                Z = -0.002117f
            },
            new Vector3{
                X = 9.26f,
                Y = 1.37f,
                Z = -0.001116f
            },
            new Vector3{
                X = 2.58f,
                Y = 12.33f,
                Z = 0.012495f
            },
            new Vector3{
                X = 2.58f,
                Y = 10.96f,
                Z = 0.026044f
            },
            new Vector3{
                X = 2.58f,
                Y = 9.59f,
                Z = 0.043154f
            },
            new Vector3{
                X = 2.58f,
                Y = 8.22f,
                Z = 0.064283f
            },
            new Vector3{
                X = 2.58f,
                Y = 6.85f,
                Z = 0.207596f
            },
            new Vector3{
                X = 2.58f,
                Y = 5.48f,
                Z = 0.06453f
            },
            new Vector3{
                X = 2.58f,
                Y = 4.11f,
                Z = 0.043447f
            },
            new Vector3{
                X = 2.58f,
                Y = 2.74f,
                Z = 0.026276f
            },
            new Vector3{
                X = 2.58f,
                Y = 1.37f,
                Z = 0.012659f
            },
            new Vector3{
                X = 4.25f,
                Y = 12.33f,
                Z = 0.005049f
            },
            new Vector3{
                X = 4.25f,
                Y = 10.96f,
                Z = 0.008764f
            },
            new Vector3{
                X = 4.25f,
                Y = 9.59f,
                Z = 0.010427f
            },
            new Vector3{
                X = 4.25f,
                Y = 8.22f,
                Z = 0.009729f
            },
            new Vector3{
                X = 4.25f,
                Y = 6.85f,
                Z = 0.008409f
            },
            new Vector3{
                X = 4.25f,
                Y = 5.48f,
                Z = 0.009643f
            },
            new Vector3{
                X = 4.25f,
                Y = 4.11f,
                Z = 0.010355f
            },
            new Vector3{
                X = 4.25f,
                Y = 2.74f,
                Z = 0.008727f
            },
            new Vector3{
                X = 4.25f,
                Y = 1.37f,
                Z = 0.005039f
            },
            new Vector3{
                X = 5.92f,
                Y = 12.33f,
                Z = -0.001362f
            },
            new Vector3{
                X = 5.92f,
                Y = 10.96f,
                Z = -0.00256f
            },
            new Vector3{
                X = 5.92f,
                Y = 9.59f,
                Z = -0.003553f
            },
            new Vector3{
                X = 5.92f,
                Y = 8.22f,
                Z = -0.004227f
            },
            new Vector3{
                X = 5.92f,
                Y = 6.85f,
                Z = -0.004469f
            },
            new Vector3{
                X = 5.92f,
                Y = 5.48f,
                Z = -0.004248f
            },
            new Vector3{
                X = 5.92f,
                Y = 4.11f,
                Z = -0.003589f
            },
            new Vector3{
                X = 5.92f,
                Y = 2.74f,
                Z = -0.002598f
            },
            new Vector3{
                X = 5.92f,
                Y = 1.37f,
                Z = -0.00139f
            },
            new Vector3{
                X = 7.59f,
                Y = 12.33f,
                Z = -0.001771f
            },
            new Vector3{
                X = 7.59f,
                Y = 10.96f,
                Z = -0.003334f
            },
            new Vector3{
                X = 7.59f,
                Y = 9.59f,
                Z = -0.004555f
            },
            new Vector3{
                X = 7.59f,
                Y = 8.22f,
                Z = -0.00533f
            },
            new Vector3{
                X = 7.59f,
                Y = 6.85f,
                Z = -0.005597f
            },
            new Vector3{
                X = 7.59f,
                Y = 5.48f,
                Z = -0.005335f
            },
            new Vector3{
                X = 7.59f,
                Y = 4.11f,
                Z = -0.004564f
            },
            new Vector3{
                X = 7.59f,
                Y = 2.74f,
                Z = -0.003344f
            },
            new Vector3{
                X = 7.59f,
                Y = 1.37f,
                Z = -0.001778f
            },
            new Vector3{
                X = 1.09f,
                Y = 0.0f,
                Z = 0.002251f
            },
            new Vector3{
                X = 1.09f,
                Y = 13.7f,
                Z = 0.002207f
            },
            new Vector3{
                X = 2.76f,
                Y = 0.0f,
                Z = -8.5e-05f
            },
            new Vector3{
                X = 2.76f,
                Y = 13.7f,
                Z = -0.000109f
            },
            new Vector3{
                X = 4.43f,
                Y = 0.0f,
                Z = -0.000393f
            },
            new Vector3{
                X = 4.43f,
                Y = 13.7f,
                Z = -0.000382f
            },
            new Vector3{
                X = 6.1f,
                Y = 0.0f,
                Z = -3.8e-05f
            },
            new Vector3{
                X = 6.1f,
                Y = 13.7f,
                Z = -3.2e-05f
            },
            new Vector3{
                X = 7.77f,
                Y = 0.0f,
                Z = 1.5e-05f
            },
            new Vector3{
                X = 7.77f,
                Y = 13.7f,
                Z = 1.6e-05f
            },
            new Vector3{
                X = 9.44f,
                Y = 0.0f,
                Z = 4.3e-05f
            },
            new Vector3{
                X = 9.44f,
                Y = 13.7f,
                Z = 4.3e-05f
            },
            new Vector3{
                X = 1.09f,
                Y = 12.33f,
                Z = -0.008584f
            },
            new Vector3{
                X = 1.09f,
                Y = 10.96f,
                Z = -0.01929f
            },
            new Vector3{
                X = 1.09f,
                Y = 9.59f,
                Z = -0.031418f
            },
            new Vector3{
                X = 1.09f,
                Y = 8.22f,
                Z = -0.045943f
            },
            new Vector3{
                X = 1.09f,
                Y = 6.85f,
                Z = -0.059976f
            },
            new Vector3{
                X = 1.09f,
                Y = 5.48f,
                Z = -0.046297f
            },
            new Vector3{
                X = 1.09f,
                Y = 4.11f,
                Z = -0.03171f
            },
            new Vector3{
                X = 1.09f,
                Y = 2.74f,
                Z = -0.019503f
            },
            new Vector3{
                X = 1.09f,
                Y = 1.37f,
                Z = -0.008698f
            },
            new Vector3{
                X = 9.44f,
                Y = 12.33f,
                Z = -0.001013f
            },
            new Vector3{
                X = 9.44f,
                Y = 10.96f,
                Z = -0.001976f
            },
            new Vector3{
                X = 9.44f,
                Y = 9.59f,
                Z = -0.002749f
            },
            new Vector3{
                X = 9.44f,
                Y = 8.22f,
                Z = -0.003251f
            },
            new Vector3{
                X = 9.44f,
                Y = 6.85f,
                Z = -0.003424f
            },
            new Vector3{
                X = 9.44f,
                Y = 5.48f,
                Z = -0.00325f
            },
            new Vector3{
                X = 9.44f,
                Y = 4.11f,
                Z = -0.002747f
            },
            new Vector3{
                X = 9.44f,
                Y = 2.74f,
                Z = -0.001973f
            },
            new Vector3{
                X = 9.44f,
                Y = 1.37f,
                Z = -0.00101f
            },
            new Vector3{
                X = 2.76f,
                Y = 12.33f,
                Z = 0.012434f
            },
            new Vector3{
                X = 2.76f,
                Y = 10.96f,
                Z = 0.026106f
            },
            new Vector3{
                X = 2.76f,
                Y = 9.59f,
                Z = 0.042051f
            },
            new Vector3{
                X = 2.76f,
                Y = 8.22f,
                Z = 0.058267f
            },
            new Vector3{
                X = 2.76f,
                Y = 6.85f,
                Z = 0.068166f
            },
            new Vector3{
                X = 2.76f,
                Y = 5.48f,
                Z = 0.058363f
            },
            new Vector3{
                X = 2.76f,
                Y = 4.11f,
                Z = 0.042291f
            },
            new Vector3{
                X = 2.76f,
                Y = 2.74f,
                Z = 0.026329f
            },
            new Vector3{
                X = 2.76f,
                Y = 1.37f,
                Z = 0.012596f
            },
            new Vector3{
                X = 4.43f,
                Y = 12.33f,
                Z = 0.003766f
            },
            new Vector3{
                X = 4.43f,
                Y = 10.96f,
                Z = 0.006686f
            },
            new Vector3{
                X = 4.43f,
                Y = 9.59f,
                Z = 0.007716f
            },
            new Vector3{
                X = 4.43f,
                Y = 8.22f,
                Z = 0.007039f
            },
            new Vector3{
                X = 4.43f,
                Y = 6.85f,
                Z = 0.006266f
            },
            new Vector3{
                X = 4.43f,
                Y = 5.48f,
                Z = 0.006971f
            },
            new Vector3{
                X = 4.43f,
                Y = 4.11f,
                Z = 0.007641f
            },
            new Vector3{
                X = 4.43f,
                Y = 2.74f,
                Z = 0.006631f
            },
            new Vector3{
                X = 4.43f,
                Y = 1.37f,
                Z = 0.003729f
            },
            new Vector3{
                X = 6.1f,
                Y = 12.33f,
                Z = -0.001548f
            },
            new Vector3{
                X = 6.1f,
                Y = 10.96f,
                Z = -0.002904f
            },
            new Vector3{
                X = 6.1f,
                Y = 9.59f,
                Z = -0.003987f
            },
            new Vector3{
                X = 6.1f,
                Y = 8.22f,
                Z = -0.004696f
            },
            new Vector3{
                X = 6.1f,
                Y = 6.85f,
                Z = -0.004947f
            },
            new Vector3{
                X = 6.1f,
                Y = 5.48f,
                Z = -0.004714f
            },
            new Vector3{
                X = 6.1f,
                Y = 4.11f,
                Z = -0.004019f
            },
            new Vector3{
                X = 6.1f,
                Y = 2.74f,
                Z = -0.00294f
            },
            new Vector3{
                X = 6.1f,
                Y = 1.37f,
                Z = -0.001576f
            },
            new Vector3{
                X = 7.77f,
                Y = 12.33f,
                Z = -0.001709f
            },
            new Vector3{
                X = 7.77f,
                Y = 10.96f,
                Z = -0.003236f
            },
            new Vector3{
                X = 7.77f,
                Y = 9.59f,
                Z = -0.004427f
            },
            new Vector3{
                X = 7.77f,
                Y = 8.22f,
                Z = -0.005183f
            },
            new Vector3{
                X = 7.77f,
                Y = 6.85f,
                Z = -0.005443f
            },
            new Vector3{
                X = 7.77f,
                Y = 5.48f,
                Z = -0.005187f
            },
            new Vector3{
                X = 7.77f,
                Y = 4.11f,
                Z = -0.004434f
            },
            new Vector3{
                X = 7.77f,
                Y = 2.74f,
                Z = -0.003243f
            },
            new Vector3{
                X = 7.77f,
                Y = 1.37f,
                Z = -0.001713f
            },
            new Vector3{
                X = 0.0f,
                Y = 0.25f,
                Z = -0.033037f
            },
            new Vector3{
                X = 1.0f,
                Y = 0.25f,
                Z = -0.002163f
            },
            new Vector3{
                X = 2.67f,
                Y = 0.25f,
                Z = 0.002329f
            },
            new Vector3{
                X = 4.34f,
                Y = 0.25f,
                Z = 0.00083f
            },
            new Vector3{
                X = 6.01f,
                Y = 0.25f,
                Z = -0.000285f
            },
            new Vector3{
                X = 7.68f,
                Y = 0.25f,
                Z = -0.000326f
            },
            new Vector3{
                X = 9.35f,
                Y = 0.25f,
                Z = -0.000195f
            },
            new Vector3{
                X = 10.35f,
                Y = 0.25f,
                Z = 0.000335f
            },
            new Vector3{
                X = 0.91f,
                Y = 0.25f,
                Z = -0.004766f
            },
            new Vector3{
                X = 2.58f,
                Y = 0.25f,
                Z = 0.002605f
            },
            new Vector3{
                X = 4.25f,
                Y = 0.25f,
                Z = 0.001247f
            },
            new Vector3{
                X = 5.92f,
                Y = 0.25f,
                Z = -0.000273f
            },
            new Vector3{
                X = 7.59f,
                Y = 0.25f,
                Z = -0.00035f
            },
            new Vector3{
                X = 9.26f,
                Y = 0.25f,
                Z = -0.000233f
            },
            new Vector3{
                X = 1.09f,
                Y = 0.25f,
                Z = 0.00014f
            },
            new Vector3{
                X = 2.76f,
                Y = 0.25f,
                Z = 0.002219f
            },
            new Vector3{
                X = 4.43f,
                Y = 0.25f,
                Z = 0.00041f
            },
            new Vector3{
                X = 6.1f,
                Y = 0.25f,
                Z = -0.000325f
            },
            new Vector3{
                X = 7.77f,
                Y = 0.25f,
                Z = -0.000307f
            },
            new Vector3{
                X = 9.44f,
                Y = 0.25f,
                Z = -0.000151f
            },
            new Vector3{
                X = 0.0f,
                Y = 0.5f,
                Z = -0.035792f
            },
            new Vector3{
                X = 1.0f,
                Y = 0.5f,
                Z = -0.004294f
            },
            new Vector3{
                X = 2.67f,
                Y = 0.5f,
                Z = 0.004628f
            },
            new Vector3{
                X = 4.34f,
                Y = 0.5f,
                Z = 0.001654f
            },
            new Vector3{
                X = 6.01f,
                Y = 0.5f,
                Z = -0.000564f
            },
            new Vector3{
                X = 7.68f,
                Y = 0.5f,
                Z = -0.00065f
            },
            new Vector3{
                X = 9.35f,
                Y = 0.5f,
                Z = -0.000391f
            },
            new Vector3{
                X = 10.35f,
                Y = 0.5f,
                Z = 0.000153f
            },
            new Vector3{
                X = 0.91f,
                Y = 0.5f,
                Z = -0.006956f
            },
            new Vector3{
                X = 2.58f,
                Y = 0.5f,
                Z = 0.004825f
            },
            new Vector3{
                X = 4.25f,
                Y = 0.5f,
                Z = 0.002115f
            },
            new Vector3{
                X = 5.92f,
                Y = 0.5f,
                Z = -0.00053f
            },
            new Vector3{
                X = 7.59f,
                Y = 0.5f,
                Z = -0.000675f
            },
            new Vector3{
                X = 9.26f,
                Y = 0.5f,
                Z = -0.000432f
            },
            new Vector3{
                X = 1.09f,
                Y = 0.5f,
                Z = -0.001877f
            },
            new Vector3{
                X = 2.76f,
                Y = 0.5f,
                Z = 0.004517f
            },
            new Vector3{
                X = 4.43f,
                Y = 0.5f,
                Z = 0.001195f
            },
            new Vector3{
                X = 6.1f,
                Y = 0.5f,
                Z = -0.000611f
            },
            new Vector3{
                X = 7.77f,
                Y = 0.5f,
                Z = -0.000628f
            },
            new Vector3{
                X = 9.44f,
                Y = 0.5f,
                Z = -0.000345f
            },
            new Vector3{
                X = 0.0f,
                Y = 0.79f,
                Z = -0.039191f
            },
            new Vector3{
                X = 0.91f,
                Y = 0.79f,
                Z = -0.009503f
            },
            new Vector3{
                X = 0.0f,
                Y = 1.08f,
                Z = -0.042849f
            },
            new Vector3{
                X = 0.91f,
                Y = 1.08f,
                Z = -0.012068f
            },
            new Vector3{
                X = 1.0f,
                Y = 0.79f,
                Z = -0.006742f
            },
            new Vector3{
                X = 1.0f,
                Y = 1.08f,
                Z = -0.009177f
            },
            new Vector3{
                X = 1.09f,
                Y = 0.79f,
                Z = -0.004182f
            },
            new Vector3{
                X = 1.09f,
                Y = 1.08f,
                Z = -0.006449f
            },
            new Vector3{
                X = 2.58f,
                Y = 0.79f,
                Z = 0.007413f
            },
            new Vector3{
                X = 2.58f,
                Y = 1.08f,
                Z = 0.010017f
            },
            new Vector3{
                X = 2.67f,
                Y = 0.79f,
                Z = 0.007289f
            },
            new Vector3{
                X = 2.67f,
                Y = 1.08f,
                Z = 0.009961f
            },
            new Vector3{
                X = 2.76f,
                Y = 0.79f,
                Z = 0.007187f
            },
            new Vector3{
                X = 2.76f,
                Y = 1.08f,
                Z = 0.009875f
            },
            new Vector3{
                X = 4.25f,
                Y = 0.79f,
                Z = 0.003113f
            },
            new Vector3{
                X = 4.25f,
                Y = 1.08f,
                Z = 0.004093f
            },
            new Vector3{
                X = 4.34f,
                Y = 0.79f,
                Z = 0.002593f
            },
            new Vector3{
                X = 4.34f,
                Y = 1.08f,
                Z = 0.003503f
            },
            new Vector3{
                X = 4.43f,
                Y = 0.79f,
                Z = 0.002082f
            },
            new Vector3{
                X = 4.43f,
                Y = 1.08f,
                Z = 0.00293f
            },
            new Vector3{
                X = 5.92f,
                Y = 0.79f,
                Z = -0.000824f
            },
            new Vector3{
                X = 5.92f,
                Y = 1.08f,
                Z = -0.001111f
            },
            new Vector3{
                X = 6.01f,
                Y = 0.79f,
                Z = -0.00088f
            },
            new Vector3{
                X = 6.01f,
                Y = 1.08f,
                Z = -0.001188f
            },
            new Vector3{
                X = 6.1f,
                Y = 0.79f,
                Z = -0.000938f
            },
            new Vector3{
                X = 6.1f,
                Y = 1.08f,
                Z = -0.001261f
            },
            new Vector3{
                X = 7.59f,
                Y = 0.79f,
                Z = -0.001048f
            },
            new Vector3{
                X = 7.59f,
                Y = 1.08f,
                Z = -0.001417f
            },
            new Vector3{
                X = 7.68f,
                Y = 0.79f,
                Z = -0.001022f
            },
            new Vector3{
                X = 7.68f,
                Y = 1.08f,
                Z = -0.001388f
            },
            new Vector3{
                X = 7.77f,
                Y = 0.79f,
                Z = -0.000996f
            },
            new Vector3{
                X = 7.77f,
                Y = 1.08f,
                Z = -0.001359f
            },
            new Vector3{
                X = 9.26f,
                Y = 0.79f,
                Z = -0.000662f
            },
            new Vector3{
                X = 9.26f,
                Y = 1.08f,
                Z = -0.00089f
            },
            new Vector3{
                X = 9.35f,
                Y = 0.79f,
                Z = -0.000617f
            },
            new Vector3{
                X = 9.35f,
                Y = 1.08f,
                Z = -0.000842f
            },
            new Vector3{
                X = 9.44f,
                Y = 0.79f,
                Z = -0.000569f
            },
            new Vector3{
                X = 9.44f,
                Y = 1.08f,
                Z = -0.000791f
            },
            new Vector3{
                X = 10.35f,
                Y = 0.79f,
                Z = -5.4e-05f
            },
            new Vector3{
                X = 10.35f,
                Y = 1.08f,
                Z = -0.000254f
            },
            new Vector3{
                X = 0.0f,
                Y = 5.754f,
                Z = -0.101793f
            },
            new Vector3{
                X = 0.91f,
                Y = 5.754f,
                Z = -0.058847f
            },
            new Vector3{
                X = 0.0f,
                Y = 6.028f,
                Z = -0.102453f
            },
            new Vector3{
                X = 0.91f,
                Y = 6.028f,
                Z = -0.061134f
            },
            new Vector3{
                X = 0.0f,
                Y = 6.302f,
                Z = -0.1028f
            },
            new Vector3{
                X = 0.91f,
                Y = 6.302f,
                Z = -0.062978f
            },
            new Vector3{
                X = 0.0f,
                Y = 6.576f,
                Z = -0.102951f
            },
            new Vector3{
                X = 0.91f,
                Y = 6.576f,
                Z = -0.064183f
            },
            new Vector3{
                X = 1.0f,
                Y = 5.754f,
                Z = -0.054538f
            },
            new Vector3{
                X = 1.0f,
                Y = 6.028f,
                Z = -0.05719f
            },
            new Vector3{
                X = 1.0f,
                Y = 6.302f,
                Z = -0.059403f
            },
            new Vector3{
                X = 1.0f,
                Y = 6.576f,
                Z = -0.060884f
            },
            new Vector3{
                X = 1.09f,
                Y = 5.754f,
                Z = -0.049687f
            },
            new Vector3{
                X = 1.09f,
                Y = 6.028f,
                Z = -0.053227f
            },
            new Vector3{
                X = 1.09f,
                Y = 6.302f,
                Z = -0.056588f
            },
            new Vector3{
                X = 1.09f,
                Y = 6.576f,
                Z = -0.059092f
            },
            new Vector3{
                X = 2.58f,
                Y = 5.754f,
                Z = 0.068691f
            },
            new Vector3{
                X = 2.58f,
                Y = 6.028f,
                Z = 0.07221f
            },
            new Vector3{
                X = 2.58f,
                Y = 6.302f,
                Z = 0.075743f
            },
            new Vector3{
                X = 2.58f,
                Y = 6.576f,
                Z = 0.055848f
            },
            new Vector3{
                X = 2.67f,
                Y = 5.754f,
                Z = 0.065651f
            },
            new Vector3{
                X = 2.67f,
                Y = 6.028f,
                Z = 0.069029f
            },
            new Vector3{
                X = 2.67f,
                Y = 6.302f,
                Z = 0.072415f
            },
            new Vector3{
                X = 2.67f,
                Y = 6.576f,
                Z = 0.076816f
            },
            new Vector3{
                X = 2.76f,
                Y = 5.754f,
                Z = 0.060833f
            },
            new Vector3{
                X = 2.76f,
                Y = 6.028f,
                Z = 0.062751f
            },
            new Vector3{
                X = 2.76f,
                Y = 6.302f,
                Z = 0.064109f
            },
            new Vector3{
                X = 2.76f,
                Y = 6.576f,
                Z = 0.06576f
            },
            new Vector3{
                X = 4.25f,
                Y = 5.754f,
                Z = 0.009314f
            },
            new Vector3{
                X = 4.25f,
                Y = 6.028f,
                Z = 0.008974f
            },
            new Vector3{
                X = 4.25f,
                Y = 6.302f,
                Z = 0.008675f
            },
            new Vector3{
                X = 4.25f,
                Y = 6.576f,
                Z = 0.008472f
            },
            new Vector3{
                X = 4.34f,
                Y = 5.754f,
                Z = 0.007984f
            },
            new Vector3{
                X = 4.34f,
                Y = 6.028f,
                Z = 0.007746f
            },
            new Vector3{
                X = 4.34f,
                Y = 6.302f,
                Z = 0.007548f
            },
            new Vector3{
                X = 4.34f,
                Y = 6.576f,
                Z = 0.007419f
            },
            new Vector3{
                X = 4.43f,
                Y = 5.754f,
                Z = 0.006759f
            },
            new Vector3{
                X = 4.43f,
                Y = 6.028f,
                Z = 0.006561f
            },
            new Vector3{
                X = 4.43f,
                Y = 6.302f,
                Z = 0.006399f
            },
            new Vector3{
                X = 4.43f,
                Y = 6.576f,
                Z = 0.006296f
            },
            new Vector3{
                X = 5.92f,
                Y = 5.754f,
                Z = -0.004329f
            },
            new Vector3{
                X = 5.92f,
                Y = 6.028f,
                Z = -0.004392f
            },
            new Vector3{
                X = 5.92f,
                Y = 6.302f,
                Z = -0.004437f
            },
            new Vector3{
                X = 5.92f,
                Y = 6.576f,
                Z = -0.004462f
            },
            new Vector3{
                X = 6.01f,
                Y = 5.754f,
                Z = -0.004579f
            },
            new Vector3{
                X = 6.01f,
                Y = 6.028f,
                Z = -0.004644f
            },
            new Vector3{
                X = 6.01f,
                Y = 6.302f,
                Z = -0.004689f
            },
            new Vector3{
                X = 6.01f,
                Y = 6.576f,
                Z = -0.004716f
            },
            new Vector3{
                X = 6.1f,
                Y = 5.754f,
                Z = -0.004799f
            },
            new Vector3{
                X = 6.1f,
                Y = 6.028f,
                Z = -0.004865f
            },
            new Vector3{
                X = 6.1f,
                Y = 6.302f,
                Z = -0.004912f
            },
            new Vector3{
                X = 6.1f,
                Y = 6.576f,
                Z = -0.004939f
            },
            new Vector3{
                X = 7.59f,
                Y = 5.754f,
                Z = -0.00543f
            },
            new Vector3{
                X = 7.59f,
                Y = 6.028f,
                Z = -0.005503f
            },
            new Vector3{
                X = 7.59f,
                Y = 6.302f,
                Z = -0.005555f
            },
            new Vector3{
                X = 7.59f,
                Y = 6.576f,
                Z = -0.005587f
            },
            new Vector3{
                X = 7.68f,
                Y = 5.754f,
                Z = -0.005357f
            },
            new Vector3{
                X = 7.68f,
                Y = 6.028f,
                Z = -0.00543f
            },
            new Vector3{
                X = 7.68f,
                Y = 6.302f,
                Z = -0.005482f
            },
            new Vector3{
                X = 7.68f,
                Y = 6.576f,
                Z = -0.005513f
            },
            new Vector3{
                X = 7.77f,
                Y = 5.754f,
                Z = -0.00528f
            },
            new Vector3{
                X = 7.77f,
                Y = 6.028f,
                Z = -0.005351f
            },
            new Vector3{
                X = 7.77f,
                Y = 6.302f,
                Z = -0.005403f
            },
            new Vector3{
                X = 7.77f,
                Y = 6.576f,
                Z = -0.005434f
            },
            new Vector3{
                X = 9.26f,
                Y = 5.754f,
                Z = -0.003518f
            },
            new Vector3{
                X = 9.26f,
                Y = 6.028f,
                Z = -0.003569f
            },
            new Vector3{
                X = 9.26f,
                Y = 6.302f,
                Z = -0.003606f
            },
            new Vector3{
                X = 9.26f,
                Y = 6.576f,
                Z = -0.003628f
            },
            new Vector3{
                X = 9.35f,
                Y = 5.754f,
                Z = -0.003414f
            },
            new Vector3{
                X = 9.35f,
                Y = 6.028f,
                Z = -0.003464f
            },
            new Vector3{
                X = 9.35f,
                Y = 6.302f,
                Z = -0.003499f
            },
            new Vector3{
                X = 9.35f,
                Y = 6.576f,
                Z = -0.003521f
            },
            new Vector3{
                X = 9.44f,
                Y = 5.754f,
                Z = -0.003312f
            },
            new Vector3{
                X = 9.44f,
                Y = 6.028f,
                Z = -0.003361f
            },
            new Vector3{
                X = 9.44f,
                Y = 6.302f,
                Z = -0.003396f
            },
            new Vector3{
                X = 9.44f,
                Y = 6.576f,
                Z = -0.003417f
            },
            new Vector3{
                X = 10.35f,
                Y = 5.754f,
                Z = -0.002319f
            },
            new Vector3{
                X = 10.35f,
                Y = 6.028f,
                Z = -0.002357f
            },
            new Vector3{
                X = 10.35f,
                Y = 6.302f,
                Z = -0.002385f
            },
            new Vector3{
                X = 10.35f,
                Y = 6.576f,
                Z = -0.002402f
            },
            new Vector3{
                X = 0.0f,
                Y = 7.124f,
                Z = -0.102934f
            },
            new Vector3{
                X = 0.91f,
                Y = 7.124f,
                Z = -0.064094f
            },
            new Vector3{
                X = 0.0f,
                Y = 7.398f,
                Z = -0.102761f
            },
            new Vector3{
                X = 0.91f,
                Y = 7.398f,
                Z = -0.06281f
            },
            new Vector3{
                X = 0.0f,
                Y = 7.672f,
                Z = -0.102384f
            },
            new Vector3{
                X = 0.91f,
                Y = 7.672f,
                Z = -0.060906f
            },
            new Vector3{
                X = 0.0f,
                Y = 7.946f,
                Z = -0.101684f
            },
            new Vector3{
                X = 0.91f,
                Y = 7.946f,
                Z = -0.058577f
            },
            new Vector3{
                X = 1.0f,
                Y = 7.124f,
                Z = -0.060776f
            },
            new Vector3{
                X = 1.0f,
                Y = 7.398f,
                Z = -0.059204f
            },
            new Vector3{
                X = 1.0f,
                Y = 7.672f,
                Z = -0.056928f
            },
            new Vector3{
                X = 1.0f,
                Y = 7.946f,
                Z = -0.054236f
            },
            new Vector3{
                X = 1.09f,
                Y = 7.124f,
                Z = -0.058917f
            },
            new Vector3{
                X = 1.09f,
                Y = 7.398f,
                Z = -0.056288f
            },
            new Vector3{
                X = 1.09f,
                Y = 7.672f,
                Z = -0.05287f
            },
            new Vector3{
                X = 1.09f,
                Y = 7.946f,
                Z = -0.049321f
            },
            new Vector3{
                X = 2.58f,
                Y = 7.124f,
                Z = 0.048058f
            },
            new Vector3{
                X = 2.58f,
                Y = 7.398f,
                Z = 0.077031f
            },
            new Vector3{
                X = 2.58f,
                Y = 7.672f,
                Z = 0.072091f
            },
            new Vector3{
                X = 2.58f,
                Y = 7.946f,
                Z = 0.068508f
            },
            new Vector3{
                X = 2.67f,
                Y = 7.124f,
                Z = 0.076596f
            },
            new Vector3{
                X = 2.67f,
                Y = 7.398f,
                Z = 0.072232f
            },
            new Vector3{
                X = 2.67f,
                Y = 7.672f,
                Z = 0.068926f
            },
            new Vector3{
                X = 2.67f,
                Y = 7.946f,
                Z = 0.065504f
            },
            new Vector3{
                X = 2.76f,
                Y = 7.124f,
                Z = 0.065827f
            },
            new Vector3{
                X = 2.76f,
                Y = 7.398f,
                Z = 0.064183f
            },
            new Vector3{
                X = 2.76f,
                Y = 7.672f,
                Z = 0.062807f
            },
            new Vector3{
                X = 2.76f,
                Y = 7.946f,
                Z = 0.060809f
            },
            new Vector3{
                X = 4.25f,
                Y = 7.124f,
                Z = 0.0085f
            },
            new Vector3{
                X = 4.25f,
                Y = 7.398f,
                Z = 0.008727f
            },
            new Vector3{
                X = 4.25f,
                Y = 7.672f,
                Z = 0.009043f
            },
            new Vector3{
                X = 4.25f,
                Y = 7.946f,
                Z = 0.009394f
            },
            new Vector3{
                X = 4.34f,
                Y = 7.124f,
                Z = 0.00744f
            },
            new Vector3{
                X = 4.34f,
                Y = 7.398f,
                Z = 0.007587f
            },
            new Vector3{
                X = 4.34f,
                Y = 7.672f,
                Z = 0.007801f
            },
            new Vector3{
                X = 4.34f,
                Y = 7.946f,
                Z = 0.008051f
            },
            new Vector3{
                X = 4.43f,
                Y = 7.124f,
                Z = 0.006314f
            },
            new Vector3{
                X = 4.43f,
                Y = 7.398f,
                Z = 0.006433f
            },
            new Vector3{
                X = 4.43f,
                Y = 7.672f,
                Z = 0.006609f
            },
            new Vector3{
                X = 4.43f,
                Y = 7.946f,
                Z = 0.006819f
            },
            new Vector3{
                X = 5.92f,
                Y = 7.124f,
                Z = -0.004458f
            },
            new Vector3{
                X = 5.92f,
                Y = 7.398f,
                Z = -0.004428f
            },
            new Vector3{
                X = 5.92f,
                Y = 7.672f,
                Z = -0.004379f
            },
            new Vector3{
                X = 5.92f,
                Y = 7.946f,
                Z = -0.004312f
            },
            new Vector3{
                X = 6.01f,
                Y = 7.124f,
                Z = -0.004712f
            },
            new Vector3{
                X = 6.01f,
                Y = 7.398f,
                Z = -0.004681f
            },
            new Vector3{
                X = 6.01f,
                Y = 7.672f,
                Z = -0.004632f
            },
            new Vector3{
                X = 6.01f,
                Y = 7.946f,
                Z = -0.004563f
            },
            new Vector3{
                X = 6.1f,
                Y = 7.124f,
                Z = -0.004935f
            },
            new Vector3{
                X = 6.1f,
                Y = 7.398f,
                Z = -0.004904f
            },
            new Vector3{
                X = 6.1f,
                Y = 7.672f,
                Z = -0.004854f
            },
            new Vector3{
                X = 6.1f,
                Y = 7.946f,
                Z = -0.004784f
            },
            new Vector3{
                X = 7.59f,
                Y = 7.124f,
                Z = -0.005586f
            },
            new Vector3{
                X = 7.59f,
                Y = 7.398f,
                Z = -0.005553f
            },
            new Vector3{
                X = 7.59f,
                Y = 7.672f,
                Z = -0.0055f
            },
            new Vector3{
                X = 7.59f,
                Y = 7.946f,
                Z = -0.005425f
            },
            new Vector3{
                X = 7.68f,
                Y = 7.124f,
                Z = -0.005512f
            },
            new Vector3{
                X = 7.68f,
                Y = 7.398f,
                Z = -0.00548f
            },
            new Vector3{
                X = 7.68f,
                Y = 7.672f,
                Z = -0.005427f
            },
            new Vector3{
                X = 7.68f,
                Y = 7.946f,
                Z = -0.005353f
            },
            new Vector3{
                X = 7.77f,
                Y = 7.124f,
                Z = -0.005433f
            },
            new Vector3{
                X = 7.77f,
                Y = 7.398f,
                Z = -0.005401f
            },
            new Vector3{
                X = 7.77f,
                Y = 7.672f,
                Z = -0.005349f
            },
            new Vector3{
                X = 7.77f,
                Y = 7.946f,
                Z = -0.005276f
            },
            new Vector3{
                X = 9.26f,
                Y = 7.124f,
                Z = -0.003628f
            },
            new Vector3{
                X = 9.26f,
                Y = 7.398f,
                Z = -0.003606f
            },
            new Vector3{
                X = 9.26f,
                Y = 7.672f,
                Z = -0.00357f
            },
            new Vector3{
                X = 9.26f,
                Y = 7.946f,
                Z = -0.003519f
            },
            new Vector3{
                X = 9.35f,
                Y = 7.124f,
                Z = -0.003521f
            },
            new Vector3{
                X = 9.35f,
                Y = 7.398f,
                Z = -0.0035f
            },
            new Vector3{
                X = 9.35f,
                Y = 7.672f,
                Z = -0.003464f
            },
            new Vector3{
                X = 9.35f,
                Y = 7.946f,
                Z = -0.003415f
            },
            new Vector3{
                X = 9.44f,
                Y = 7.124f,
                Z = -0.003417f
            },
            new Vector3{
                X = 9.44f,
                Y = 7.398f,
                Z = -0.003397f
            },
            new Vector3{
                X = 9.44f,
                Y = 7.672f,
                Z = -0.003362f
            },
            new Vector3{
                X = 9.44f,
                Y = 7.946f,
                Z = -0.003313f
            },
            new Vector3{
                X = 10.35f,
                Y = 7.124f,
                Z = -0.002402f
            },
            new Vector3{
                X = 10.35f,
                Y = 7.398f,
                Z = -0.002386f
            },
            new Vector3{
                X = 10.35f,
                Y = 7.672f,
                Z = -0.00236f
            },
            new Vector3{
                X = 10.35f,
                Y = 7.946f,
                Z = -0.002322f
            },
            new Vector3{
                X = 1.3383f,
                Y = 0.0f,
                Z = 0.006146f
            },
            new Vector3{
                X = 1.5867f,
                Y = 0.0f,
                Z = 0.007361f
            },
            new Vector3{
                X = 1.835f,
                Y = 0.0f,
                Z = 0.006677f
            },
            new Vector3{
                X = 2.0833f,
                Y = 0.0f,
                Z = 0.004784f
            },
            new Vector3{
                X = 2.3317f,
                Y = 0.0f,
                Z = 0.002396f
            },
            new Vector3{
                X = 1.3383f,
                Y = 0.25f,
                Z = 0.004464f
            },
            new Vector3{
                X = 1.5867f,
                Y = 0.25f,
                Z = 0.006401f
            },
            new Vector3{
                X = 1.835f,
                Y = 0.25f,
                Z = 0.006573f
            },
            new Vector3{
                X = 2.0833f,
                Y = 0.25f,
                Z = 0.005565f
            },
            new Vector3{
                X = 2.3317f,
                Y = 0.25f,
                Z = 0.003996f
            },
            new Vector3{
                X = 3.0083f,
                Y = 0.0f,
                Z = 4.4e-05f
            },
            new Vector3{
                X = 3.2567f,
                Y = 0.0f,
                Z = 0.000424f
            },
            new Vector3{
                X = 3.505f,
                Y = 0.0f,
                Z = 0.000815f
            },
            new Vector3{
                X = 3.7533f,
                Y = 0.0f,
                Z = 0.001045f
            },
            new Vector3{
                X = 4.0017f,
                Y = 0.0f,
                Z = 0.000963f
            },
            new Vector3{
                X = 3.0083f,
                Y = 0.25f,
                Z = 0.002185f
            },
            new Vector3{
                X = 3.2567f,
                Y = 0.25f,
                Z = 0.002319f
            },
            new Vector3{
                X = 3.505f,
                Y = 0.25f,
                Z = 0.002424f
            },
            new Vector3{
                X = 3.7533f,
                Y = 0.25f,
                Z = 0.002363f
            },
            new Vector3{
                X = 4.0017f,
                Y = 0.25f,
                Z = 0.002015f
            },
            new Vector3{
                X = 4.6783f,
                Y = 0.0f,
                Z = -0.001047f
            },
            new Vector3{
                X = 4.9267f,
                Y = 0.0f,
                Z = -0.001213f
            },
            new Vector3{
                X = 5.175f,
                Y = 0.0f,
                Z = -0.001048f
            },
            new Vector3{
                X = 5.4233f,
                Y = 0.0f,
                Z = -0.000691f
            },
            new Vector3{
                X = 5.6717f,
                Y = 0.0f,
                Z = -0.000286f
            },
            new Vector3{
                X = 4.6783f,
                Y = 0.25f,
                Z = -0.000375f
            },
            new Vector3{
                X = 4.9267f,
                Y = 0.25f,
                Z = -0.000728f
            },
            new Vector3{
                X = 5.175f,
                Y = 0.25f,
                Z = -0.000769f
            },
            new Vector3{
                X = 5.4233f,
                Y = 0.25f,
                Z = -0.000619f
            },
            new Vector3{
                X = 5.6717f,
                Y = 0.25f,
                Z = -0.000403f
            },
            new Vector3{
                X = 6.3483f,
                Y = 0.0f,
                Z = -0.00012f
            },
            new Vector3{
                X = 6.5967f,
                Y = 0.0f,
                Z = -0.000166f
            },
            new Vector3{
                X = 6.845f,
                Y = 0.0f,
                Z = -0.000174f
            },
            new Vector3{
                X = 7.0933f,
                Y = 0.0f,
                Z = -0.000147f
            },
            new Vector3{
                X = 7.3417f,
                Y = 0.0f,
                Z = -9.4e-05f
            },
            new Vector3{
                X = 6.3483f,
                Y = 0.25f,
                Z = -0.000415f
            },
            new Vector3{
                X = 6.5967f,
                Y = 0.25f,
                Z = -0.000468f
            },
            new Vector3{
                X = 6.845f,
                Y = 0.25f,
                Z = -0.000483f
            },
            new Vector3{
                X = 7.0933f,
                Y = 0.25f,
                Z = -0.000462f
            },
            new Vector3{
                X = 7.3417f,
                Y = 0.25f,
                Z = -0.000414f
            },
            new Vector3{
                X = 8.0183f,
                Y = 0.0f,
                Z = 1.8e-05f
            },
            new Vector3{
                X = 8.2667f,
                Y = 0.0f,
                Z = -1.2e-05f
            },
            new Vector3{
                X = 8.515f,
                Y = 0.0f,
                Z = -5.3e-05f
            },
            new Vector3{
                X = 8.7633f,
                Y = 0.0f,
                Z = -8.4e-05f
            },
            new Vector3{
                X = 9.0117f,
                Y = 0.0f,
                Z = -8.6e-05f
            },
            new Vector3{
                X = 8.0183f,
                Y = 0.25f,
                Z = -0.000287f
            },
            new Vector3{
                X = 8.2667f,
                Y = 0.25f,
                Z = -0.000295f
            },
            new Vector3{
                X = 8.515f,
                Y = 0.25f,
                Z = -0.00031f
            },
            new Vector3{
                X = 8.7633f,
                Y = 0.25f,
                Z = -0.000316f
            },
            new Vector3{
                X = 9.0117f,
                Y = 0.25f,
                Z = -0.000297f
            },
            new Vector3{
                X = 1.3383f,
                Y = 0.5f,
                Z = 0.002985f
            },
            new Vector3{
                X = 1.5867f,
                Y = 0.5f,
                Z = 0.005653f
            },
            new Vector3{
                X = 1.835f,
                Y = 0.5f,
                Z = 0.006655f
            },
            new Vector3{
                X = 2.0833f,
                Y = 0.5f,
                Z = 0.006489f
            },
            new Vector3{
                X = 2.3317f,
                Y = 0.5f,
                Z = 0.00568f
            },
            new Vector3{
                X = 3.0083f,
                Y = 0.5f,
                Z = 0.004348f
            },
            new Vector3{
                X = 3.2567f,
                Y = 0.5f,
                Z = 0.004245f
            },
            new Vector3{
                X = 3.505f,
                Y = 0.5f,
                Z = 0.004068f
            },
            new Vector3{
                X = 3.7533f,
                Y = 0.5f,
                Z = 0.003713f
            },
            new Vector3{
                X = 4.0017f,
                Y = 0.5f,
                Z = 0.003092f
            },
            new Vector3{
                X = 4.6783f,
                Y = 0.5f,
                Z = 0.000255f
            },
            new Vector3{
                X = 4.9267f,
                Y = 0.5f,
                Z = -0.000285f
            },
            new Vector3{
                X = 5.175f,
                Y = 0.5f,
                Z = -0.000526f
            },
            new Vector3{
                X = 5.4233f,
                Y = 0.5f,
                Z = -0.000572f
            },
            new Vector3{
                X = 5.6717f,
                Y = 0.5f,
                Z = -0.000532f
            },
            new Vector3{
                X = 6.3483f,
                Y = 0.5f,
                Z = -0.000715f
            },
            new Vector3{
                X = 6.5967f,
                Y = 0.5f,
                Z = -0.000777f
            },
            new Vector3{
                X = 6.845f,
                Y = 0.5f,
                Z = -0.000798f
            },
            new Vector3{
                X = 7.0933f,
                Y = 0.5f,
                Z = -0.000782f
            },
            new Vector3{
                X = 7.3417f,
                Y = 0.5f,
                Z = -0.000738f
            },
            new Vector3{
                X = 8.0183f,
                Y = 0.5f,
                Z = -0.000592f
            },
            new Vector3{
                X = 8.2667f,
                Y = 0.5f,
                Z = -0.000577f
            },
            new Vector3{
                X = 8.515f,
                Y = 0.5f,
                Z = -0.000568f
            },
            new Vector3{
                X = 8.7633f,
                Y = 0.5f,
                Z = -0.000551f
            },
            new Vector3{
                X = 9.0117f,
                Y = 0.5f,
                Z = -0.00051f
            },
            new Vector3{
                X = 1.3383f,
                Y = 0.79f,
                Z = 0.001349f
            },
            new Vector3{
                X = 1.5867f,
                Y = 0.79f,
                Z = 0.004917f
            },
            new Vector3{
                X = 1.835f,
                Y = 0.79f,
                Z = 0.006909f
            },
            new Vector3{
                X = 2.0833f,
                Y = 0.79f,
                Z = 0.007714f
            },
            new Vector3{
                X = 2.3317f,
                Y = 0.79f,
                Z = 0.00774f
            },
            new Vector3{
                X = 3.0083f,
                Y = 0.79f,
                Z = 0.006897f
            },
            new Vector3{
                X = 3.2567f,
                Y = 0.79f,
                Z = 0.006529f
            },
            new Vector3{
                X = 3.505f,
                Y = 0.79f,
                Z = 0.006014f
            },
            new Vector3{
                X = 3.7533f,
                Y = 0.79f,
                Z = 0.0053f
            },
            new Vector3{
                X = 4.0017f,
                Y = 0.79f,
                Z = 0.004345f
            },
            new Vector3{
                X = 4.6783f,
                Y = 0.79f,
                Z = 0.000952f
            },
            new Vector3{
                X = 4.9267f,
                Y = 0.79f,
                Z = 0.000189f
            },
            new Vector3{
                X = 5.175f,
                Y = 0.79f,
                Z = -0.000283f
            },
            new Vector3{
                X = 5.4233f,
                Y = 0.79f,
                Z = -0.000549f
            },
            new Vector3{
                X = 5.6717f,
                Y = 0.79f,
                Z = -0.000699f
            },
            new Vector3{
                X = 6.3483f,
                Y = 0.79f,
                Z = -0.001065f
            },
            new Vector3{
                X = 6.5967f,
                Y = 0.79f,
                Z = -0.001141f
            },
            new Vector3{
                X = 6.845f,
                Y = 0.79f,
                Z = -0.00117f
            },
            new Vector3{
                X = 7.0933f,
                Y = 0.79f,
                Z = -0.001157f
            },
            new Vector3{
                X = 7.3417f,
                Y = 0.79f,
                Z = -0.001113f
            },
            new Vector3{
                X = 8.0183f,
                Y = 0.79f,
                Z = -0.000943f
            },
            new Vector3{
                X = 8.2667f,
                Y = 0.79f,
                Z = -0.000903f
            },
            new Vector3{
                X = 8.515f,
                Y = 0.79f,
                Z = -0.000867f
            },
            new Vector3{
                X = 8.7633f,
                Y = 0.79f,
                Z = -0.000822f
            },
            new Vector3{
                X = 9.0117f,
                Y = 0.79f,
                Z = -0.000757f
            },
            new Vector3{
                X = 1.3383f,
                Y = 1.37f,
                Z = -0.001734f
            },
            new Vector3{
                X = 1.5867f,
                Y = 1.37f,
                Z = 0.003705f
            },
            new Vector3{
                X = 1.835f,
                Y = 1.37f,
                Z = 0.007727f
            },
            new Vector3{
                X = 2.0833f,
                Y = 1.37f,
                Z = 0.010461f
            },
            new Vector3{
                X = 2.3317f,
                Y = 1.37f,
                Z = 0.012053f
            },
            new Vector3{
                X = 1.3383f,
                Y = 2.74f,
                Z = -0.008642f
            },
            new Vector3{
                X = 1.5867f,
                Y = 2.74f,
                Z = 0.001585f
            },
            new Vector3{
                X = 1.835f,
                Y = 2.74f,
                Z = 0.010717f
            },
            new Vector3{
                X = 2.0833f,
                Y = 2.74f,
                Z = 0.018241f
            },
            new Vector3{
                X = 2.3317f,
                Y = 2.74f,
                Z = 0.023611f
            },
            new Vector3{
                X = 3.0083f,
                Y = 1.37f,
                Z = 0.012096f
            },
            new Vector3{
                X = 3.2567f,
                Y = 1.37f,
                Z = 0.011202f
            },
            new Vector3{
                X = 3.505f,
                Y = 1.37f,
                Z = 0.009982f
            },
            new Vector3{
                X = 3.7533f,
                Y = 1.37f,
                Z = 0.008502f
            },
            new Vector3{
                X = 4.0017f,
                Y = 1.37f,
                Z = 0.006832f
            },
            new Vector3{
                X = 3.0083f,
                Y = 2.74f,
                Z = 0.024966f
            },
            new Vector3{
                X = 3.2567f,
                Y = 2.74f,
                Z = 0.022498f
            },
            new Vector3{
                X = 3.505f,
                Y = 2.74f,
                Z = 0.019295f
            },
            new Vector3{
                X = 3.7533f,
                Y = 2.74f,
                Z = 0.015711f
            },
            new Vector3{
                X = 4.0017f,
                Y = 2.74f,
                Z = 0.012081f
            },
            new Vector3{
                X = 4.6783f,
                Y = 1.37f,
                Z = 0.002196f
            },
            new Vector3{
                X = 4.9267f,
                Y = 1.37f,
                Z = 0.000997f
            },
            new Vector3{
                X = 5.175f,
                Y = 1.37f,
                Z = 9.2e-05f
            },
            new Vector3{
                X = 5.4233f,
                Y = 1.37f,
                Z = -0.000572f
            },
            new Vector3{
                X = 5.6717f,
                Y = 1.37f,
                Z = -0.001049f
            },
            new Vector3{
                X = 4.6783f,
                Y = 2.74f,
                Z = 0.004149f
            },
            new Vector3{
                X = 4.9267f,
                Y = 2.74f,
                Z = 0.002087f
            },
            new Vector3{
                X = 5.175f,
                Y = 2.74f,
                Z = 0.000414f
            },
            new Vector3{
                X = 5.4233f,
                Y = 2.74f,
                Z = -0.000904f
            },
            new Vector3{
                X = 5.6717f,
                Y = 2.74f,
                Z = -0.001899f
            },
            new Vector3{
                X = 6.3483f,
                Y = 1.37f,
                Z = -0.00176f
            },
            new Vector3{
                X = 6.5967f,
                Y = 1.37f,
                Z = -0.001869f
            },
            new Vector3{
                X = 6.845f,
                Y = 1.37f,
                Z = -0.001913f
            },
            new Vector3{
                X = 7.0933f,
                Y = 1.37f,
                Z = -0.001904f
            },
            new Vector3{
                X = 7.3417f,
                Y = 1.37f,
                Z = -0.001855f
            },
            new Vector3{
                X = 6.3483f,
                Y = 2.74f,
                Z = -0.003265f
            },
            new Vector3{
                X = 6.5967f,
                Y = 2.74f,
                Z = -0.003456f
            },
            new Vector3{
                X = 6.845f,
                Y = 2.74f,
                Z = -0.003537f
            },
            new Vector3{
                X = 7.0933f,
                Y = 2.74f,
                Z = -0.003531f
            },
            new Vector3{
                X = 7.3417f,
                Y = 2.74f,
                Z = -0.00346f
            },
            new Vector3{
                X = 8.0183f,
                Y = 1.37f,
                Z = -0.001627f
            },
            new Vector3{
                X = 8.2667f,
                Y = 1.37f,
                Z = -0.001541f
            },
            new Vector3{
                X = 8.515f,
                Y = 1.37f,
                Z = -0.001453f
            },
            new Vector3{
                X = 8.7633f,
                Y = 1.37f,
                Z = -0.001356f
            },
            new Vector3{
                X = 9.0117f,
                Y = 1.37f,
                Z = -0.001245f
            },
            new Vector3{
                X = 8.0183f,
                Y = 2.74f,
                Z = -0.003086f
            },
            new Vector3{
                X = 8.2667f,
                Y = 2.74f,
                Z = -0.00291f
            },
            new Vector3{
                X = 8.515f,
                Y = 2.74f,
                Z = -0.002721f
            },
            new Vector3{
                X = 8.7633f,
                Y = 2.74f,
                Z = -0.002523f
            },
            new Vector3{
                X = 9.0117f,
                Y = 2.74f,
                Z = -0.00232f
            },
            new Vector3{
                X = 1.3383f,
                Y = 4.11f,
                Z = -0.015658f
            },
            new Vector3{
                X = 1.5867f,
                Y = 4.11f,
                Z = 0.001454f
            },
            new Vector3{
                X = 1.835f,
                Y = 4.11f,
                Z = 0.01759f
            },
            new Vector3{
                X = 2.0833f,
                Y = 4.11f,
                Z = 0.030887f
            },
            new Vector3{
                X = 2.3317f,
                Y = 4.11f,
                Z = 0.039877f
            },
            new Vector3{
                X = 3.0083f,
                Y = 4.11f,
                Z = 0.038451f
            },
            new Vector3{
                X = 3.2567f,
                Y = 4.11f,
                Z = 0.0332f
            },
            new Vector3{
                X = 3.505f,
                Y = 4.11f,
                Z = 0.027187f
            },
            new Vector3{
                X = 3.7533f,
                Y = 4.11f,
                Z = 0.021012f
            },
            new Vector3{
                X = 4.0017f,
                Y = 4.11f,
                Z = 0.015227f
            },
            new Vector3{
                X = 4.6783f,
                Y = 4.11f,
                Z = 0.004557f
            },
            new Vector3{
                X = 4.9267f,
                Y = 4.11f,
                Z = 0.002042f
            },
            new Vector3{
                X = 5.175f,
                Y = 4.11f,
                Z = 2.6e-05f
            },
            new Vector3{
                X = 5.4233f,
                Y = 4.11f,
                Z = -0.001551f
            },
            new Vector3{
                X = 5.6717f,
                Y = 4.11f,
                Z = -0.00274f
            },
            new Vector3{
                X = 6.3483f,
                Y = 4.11f,
                Z = -0.004438f
            },
            new Vector3{
                X = 6.5967f,
                Y = 4.11f,
                Z = -0.004689f
            },
            new Vector3{
                X = 6.845f,
                Y = 4.11f,
                Z = -0.0048f
            },
            new Vector3{
                X = 7.0933f,
                Y = 4.11f,
                Z = -0.004798f
            },
            new Vector3{
                X = 7.3417f,
                Y = 4.11f,
                Z = -0.004711f
            },
            new Vector3{
                X = 8.0183f,
                Y = 4.11f,
                Z = -0.004224f
            },
            new Vector3{
                X = 8.2667f,
                Y = 4.11f,
                Z = -0.003984f
            },
            new Vector3{
                X = 8.515f,
                Y = 4.11f,
                Z = -0.003726f
            },
            new Vector3{
                X = 8.7633f,
                Y = 4.11f,
                Z = -0.003457f
            },
            new Vector3{
                X = 9.0117f,
                Y = 4.11f,
                Z = -0.003187f
            },
            new Vector3{
                X = 1.3383f,
                Y = 5.48f,
                Z = -0.026948f
            },
            new Vector3{
                X = 1.5867f,
                Y = 5.48f,
                Z = -0.003377f
            },
            new Vector3{
                X = 1.835f,
                Y = 5.48f,
                Z = 0.02169f
            },
            new Vector3{
                X = 2.0833f,
                Y = 5.48f,
                Z = 0.044711f
            },
            new Vector3{
                X = 2.3317f,
                Y = 5.48f,
                Z = 0.060766f
            },
            new Vector3{
                X = 3.0083f,
                Y = 5.48f,
                Z = 0.047781f
            },
            new Vector3{
                X = 3.2567f,
                Y = 5.48f,
                Z = 0.037572f
            },
            new Vector3{
                X = 3.505f,
                Y = 5.48f,
                Z = 0.028468f
            },
            new Vector3{
                X = 3.7533f,
                Y = 5.48f,
                Z = 0.020767f
            },
            new Vector3{
                X = 4.0017f,
                Y = 5.48f,
                Z = 0.014505f
            },
            new Vector3{
                X = 4.6783f,
                Y = 5.48f,
                Z = 0.003917f
            },
            new Vector3{
                X = 4.9267f,
                Y = 5.48f,
                Z = 0.001421f
            },
            new Vector3{
                X = 5.175f,
                Y = 5.48f,
                Z = -0.000587f
            },
            new Vector3{
                X = 5.4233f,
                Y = 5.48f,
                Z = -0.002166f
            },
            new Vector3{
                X = 5.6717f,
                Y = 5.48f,
                Z = -0.003369f
            },
            new Vector3{
                X = 6.3483f,
                Y = 5.48f,
                Z = -0.005176f
            },
            new Vector3{
                X = 6.5967f,
                Y = 5.48f,
                Z = -0.005458f
            },
            new Vector3{
                X = 6.845f,
                Y = 5.48f,
                Z = -0.005587f
            },
            new Vector3{
                X = 7.0933f,
                Y = 5.48f,
                Z = -0.005591f
            },
            new Vector3{
                X = 7.3417f,
                Y = 5.48f,
                Z = -0.005498f
            },
            new Vector3{
                X = 8.0183f,
                Y = 5.48f,
                Z = -0.004946f
            },
            new Vector3{
                X = 8.2667f,
                Y = 5.48f,
                Z = -0.004669f
            },
            new Vector3{
                X = 8.515f,
                Y = 5.48f,
                Z = -0.00437f
            },
            new Vector3{
                X = 8.7633f,
                Y = 5.48f,
                Z = -0.00406f
            },
            new Vector3{
                X = 9.0117f,
                Y = 5.48f,
                Z = -0.00375f
            },
            new Vector3{
                X = 1.3383f,
                Y = 5.754f,
                Z = -0.032946f
            },
            new Vector3{
                X = 1.5867f,
                Y = 5.754f,
                Z = -0.011845f
            },
            new Vector3{
                X = 1.835f,
                Y = 5.754f,
                Z = 0.013257f
            },
            new Vector3{
                X = 2.0833f,
                Y = 5.754f,
                Z = 0.040023f
            },
            new Vector3{
                X = 2.3317f,
                Y = 5.754f,
                Z = 0.061809f
            },
            new Vector3{
                X = 3.0083f,
                Y = 5.754f,
                Z = 0.047845f
            },
            new Vector3{
                X = 3.2567f,
                Y = 5.754f,
                Z = 0.036357f
            },
            new Vector3{
                X = 3.505f,
                Y = 5.754f,
                Z = 0.026923f
            },
            new Vector3{
                X = 3.7533f,
                Y = 5.754f,
                Z = 0.019485f
            },
            new Vector3{
                X = 4.0017f,
                Y = 5.754f,
                Z = 0.013732f
            },
            new Vector3{
                X = 4.6783f,
                Y = 5.754f,
                Z = 0.003771f
            },
            new Vector3{
                X = 4.9267f,
                Y = 5.754f,
                Z = 0.001311f
            },
            new Vector3{
                X = 5.175f,
                Y = 5.754f,
                Z = -0.000678f
            },
            new Vector3{
                X = 5.4233f,
                Y = 5.754f,
                Z = -0.002248f
            },
            new Vector3{
                X = 5.6717f,
                Y = 5.754f,
                Z = -0.003448f
            },
            new Vector3{
                X = 6.3483f,
                Y = 5.754f,
                Z = -0.005266f
            },
            new Vector3{
                X = 6.5967f,
                Y = 5.754f,
                Z = -0.005551f
            },
            new Vector3{
                X = 6.845f,
                Y = 5.754f,
                Z = -0.005682f
            },
            new Vector3{
                X = 7.0933f,
                Y = 5.754f,
                Z = -0.005688f
            },
            new Vector3{
                X = 7.3417f,
                Y = 5.754f,
                Z = -0.005594f
            },
            new Vector3{
                X = 8.0183f,
                Y = 5.754f,
                Z = -0.005034f
            },
            new Vector3{
                X = 8.2667f,
                Y = 5.754f,
                Z = -0.004754f
            },
            new Vector3{
                X = 8.515f,
                Y = 5.754f,
                Z = -0.00445f
            },
            new Vector3{
                X = 8.7633f,
                Y = 5.754f,
                Z = -0.004135f
            },
            new Vector3{
                X = 9.0117f,
                Y = 5.754f,
                Z = -0.00382f
            },
            new Vector3{
                X = 1.3383f,
                Y = 6.85f,
                Z = -0.064664f
            },
            new Vector3{
                X = 1.5867f,
                Y = 6.85f,
                Z = -0.075394f
            },
            new Vector3{
                X = 1.835f,
                Y = 6.85f,
                Z = -0.086624f
            },
            new Vector3{
                X = 2.0833f,
                Y = 6.85f,
                Z = -0.09509f
            },
            new Vector3{
                X = 2.3317f,
                Y = 6.85f,
                Z = -0.106536f
            },
            new Vector3{
                X = 1.3383f,
                Y = 7.124f,
                Z = -0.060539f
            },
            new Vector3{
                X = 1.5867f,
                Y = 7.124f,
                Z = -0.065931f
            },
            new Vector3{
                X = 1.835f,
                Y = 7.124f,
                Z = -0.06819f
            },
            new Vector3{
                X = 2.0833f,
                Y = 7.124f,
                Z = -0.055771f
            },
            new Vector3{
                X = 2.3317f,
                Y = 7.124f,
                Z = 0.017015f
            },
            new Vector3{
                X = 3.0083f,
                Y = 6.85f,
                Z = 0.042424f
            },
            new Vector3{
                X = 3.2567f,
                Y = 6.85f,
                Z = 0.028197f
            },
            new Vector3{
                X = 3.505f,
                Y = 6.85f,
                Z = 0.019643f
            },
            new Vector3{
                X = 3.7533f,
                Y = 6.85f,
                Z = 0.014406f
            },
            new Vector3{
                X = 4.0017f,
                Y = 6.85f,
                Z = 0.011055f
            },
            new Vector3{
                X = 3.0083f,
                Y = 7.124f,
                Z = 0.043394f
            },
            new Vector3{
                X = 3.2567f,
                Y = 7.124f,
                Z = 0.029325f
            },
            new Vector3{
                X = 3.505f,
                Y = 7.124f,
                Z = 0.020524f
            },
            new Vector3{
                X = 3.7533f,
                Y = 7.124f,
                Z = 0.014977f
            },
            new Vector3{
                X = 4.0017f,
                Y = 7.124f,
                Z = 0.011342f
            },
            new Vector3{
                X = 4.6783f,
                Y = 6.85f,
                Z = 0.003476f
            },
            new Vector3{
                X = 4.9267f,
                Y = 6.85f,
                Z = 0.001115f
            },
            new Vector3{
                X = 5.175f,
                Y = 6.85f,
                Z = -0.000827f
            },
            new Vector3{
                X = 5.4233f,
                Y = 6.85f,
                Z = -0.00238f
            },
            new Vector3{
                X = 5.6717f,
                Y = 6.85f,
                Z = -0.00358f
            },
            new Vector3{
                X = 4.6783f,
                Y = 7.124f,
                Z = 0.003504f
            },
            new Vector3{
                X = 4.9267f,
                Y = 7.124f,
                Z = 0.001134f
            },
            new Vector3{
                X = 5.175f,
                Y = 7.124f,
                Z = -0.000814f
            },
            new Vector3{
                X = 5.4233f,
                Y = 7.124f,
                Z = -0.002369f
            },
            new Vector3{
                X = 5.6717f,
                Y = 7.124f,
                Z = -0.003569f
            },
            new Vector3{
                X = 6.3483f,
                Y = 6.85f,
                Z = -0.005423f
            },
            new Vector3{
                X = 6.5967f,
                Y = 6.85f,
                Z = -0.005714f
            },
            new Vector3{
                X = 6.845f,
                Y = 6.85f,
                Z = -0.00585f
            },
            new Vector3{
                X = 7.0933f,
                Y = 6.85f,
                Z = -0.005857f
            },
            new Vector3{
                X = 7.3417f,
                Y = 6.85f,
                Z = -0.005764f
            },
            new Vector3{
                X = 6.3483f,
                Y = 7.124f,
                Z = -0.005411f
            },
            new Vector3{
                X = 6.5967f,
                Y = 7.124f,
                Z = -0.005702f
            },
            new Vector3{
                X = 6.845f,
                Y = 7.124f,
                Z = -0.005838f
            },
            new Vector3{
                X = 7.0933f,
                Y = 7.124f,
                Z = -0.005846f
            },
            new Vector3{
                X = 7.3417f,
                Y = 7.124f,
                Z = -0.005753f
            },
            new Vector3{
                X = 8.0183f,
                Y = 6.85f,
                Z = -0.0051920000000000004f
            },
            new Vector3{
                X = 8.2667f,
                Y = 6.85f,
                Z = -0.004904f
            },
            new Vector3{
                X = 8.515f,
                Y = 6.85f,
                Z = -0.004592f
            },
            new Vector3{
                X = 8.7633f,
                Y = 6.85f,
                Z = -0.004268f
            },
            new Vector3{
                X = 9.0117f,
                Y = 6.85f,
                Z = -0.003945f
            },
            new Vector3{
                X = 8.0183f,
                Y = 7.124f,
                Z = -0.005182f
            },
            new Vector3{
                X = 8.2667f,
                Y = 7.124f,
                Z = -0.004894f
            },
            new Vector3{
                X = 8.515f,
                Y = 7.124f,
                Z = -0.004583f
            },
            new Vector3{
                X = 8.7633f,
                Y = 7.124f,
                Z = -0.00426f
            },
            new Vector3{
                X = 9.0117f,
                Y = 7.124f,
                Z = -0.003938f
            },
            new Vector3{
                X = 1.3383f,
                Y = 8.22f,
                Z = -0.026483f
            },
            new Vector3{
                X = 1.5867f,
                Y = 8.22f,
                Z = -0.002842f
            },
            new Vector3{
                X = 1.835f,
                Y = 8.22f,
                Z = 0.022146f
            },
            new Vector3{
                X = 2.0833f,
                Y = 8.22f,
                Z = 0.0449f
            },
            new Vector3{
                X = 2.3317f,
                Y = 8.22f,
                Z = 0.060631f
            },
            new Vector3{
                X = 1.3383f,
                Y = 9.59f,
                Z = -0.015502f
            },
            new Vector3{
                X = 1.5867f,
                Y = 9.59f,
                Z = 0.001428f
            },
            new Vector3{
                X = 1.835f,
                Y = 9.59f,
                Z = 0.017395f
            },
            new Vector3{
                X = 2.0833f,
                Y = 9.59f,
                Z = 0.030584f
            },
            new Vector3{
                X = 2.3317f,
                Y = 9.59f,
                Z = 0.039546f
            },
            new Vector3{
                X = 3.0083f,
                Y = 8.22f,
                Z = 0.047883f
            },
            new Vector3{
                X = 3.2567f,
                Y = 8.22f,
                Z = 0.037781f
            },
            new Vector3{
                X = 3.505f,
                Y = 8.22f,
                Z = 0.0287f
            },
            new Vector3{
                X = 3.7533f,
                Y = 8.22f,
                Z = 0.020964f
            },
            new Vector3{
                X = 4.0017f,
                Y = 8.22f,
                Z = 0.014642f
            },
            new Vector3{
                X = 3.0083f,
                Y = 9.59f,
                Z = 0.038286f
            },
            new Vector3{
                X = 3.2567f,
                Y = 9.59f,
                Z = 0.033106f
            },
            new Vector3{
                X = 3.505f,
                Y = 9.59f,
                Z = 0.027156f
            },
            new Vector3{
                X = 3.7533f,
                Y = 9.59f,
                Z = 0.02103f
            },
            new Vector3{
                X = 4.0017f,
                Y = 9.59f,
                Z = 0.01528f
            },
            new Vector3{
                X = 4.6783f,
                Y = 8.22f,
                Z = 0.00397f
            },
            new Vector3{
                X = 4.9267f,
                Y = 8.22f,
                Z = 0.001464f
            },
            new Vector3{
                X = 5.175f,
                Y = 8.22f,
                Z = -0.000552f
            },
            new Vector3{
                X = 5.4233f,
                Y = 8.22f,
                Z = -0.002137f
            },
            new Vector3{
                X = 5.6717f,
                Y = 8.22f,
                Z = -0.003345f
            },
            new Vector3{
                X = 4.6783f,
                Y = 9.59f,
                Z = 0.004631f
            },
            new Vector3{
                X = 4.9267f,
                Y = 9.59f,
                Z = 0.00211f
            },
            new Vector3{
                X = 5.175f,
                Y = 9.59f,
                Z = 8.6e-05f
            },
            new Vector3{
                X = 5.4233f,
                Y = 9.59f,
                Z = -0.001499f
            },
            new Vector3{
                X = 5.6717f,
                Y = 9.59f,
                Z = -0.002697f
            },
            new Vector3{
                X = 6.3483f,
                Y = 8.22f,
                Z = -0.005161f
            },
            new Vector3{
                X = 6.5967f,
                Y = 8.22f,
                Z = -0.005445f
            },
            new Vector3{
                X = 6.845f,
                Y = 8.22f,
                Z = -0.005576f
            },
            new Vector3{
                X = 7.0933f,
                Y = 8.22f,
                Z = -0.005582f
            },
            new Vector3{
                X = 7.3417f,
                Y = 8.22f,
                Z = -0.005492f
            },
            new Vector3{
                X = 6.3483f,
                Y = 9.59f,
                Z = -0.004411f
            },
            new Vector3{
                X = 6.5967f,
                Y = 9.59f,
                Z = -0.004666f
            },
            new Vector3{
                X = 6.845f,
                Y = 9.59f,
                Z = -0.004781f
            },
            new Vector3{
                X = 7.0933f,
                Y = 9.59f,
                Z = -0.004783f
            },
            new Vector3{
                X = 7.3417f,
                Y = 9.59f,
                Z = -0.0047f
            },
            new Vector3{
                X = 8.0183f,
                Y = 8.22f,
                Z = -0.004943f
            },
            new Vector3{
                X = 8.2667f,
                Y = 8.22f,
                Z = -0.004667f
            },
            new Vector3{
                X = 8.515f,
                Y = 8.22f,
                Z = -0.004369f
            },
            new Vector3{
                X = 8.7633f,
                Y = 8.22f,
                Z = -0.00406f
            },
            new Vector3{
                X = 9.0117f,
                Y = 8.22f,
                Z = -0.003751f
            },
            new Vector3{
                X = 8.0183f,
                Y = 9.59f,
                Z = -0.004219f
            },
            new Vector3{
                X = 8.2667f,
                Y = 9.59f,
                Z = -0.003981f
            },
            new Vector3{
                X = 8.515f,
                Y = 9.59f,
                Z = -0.003724f
            },
            new Vector3{
                X = 8.7633f,
                Y = 9.59f,
                Z = -0.003457f
            },
            new Vector3{
                X = 9.0117f,
                Y = 9.59f,
                Z = -0.003188f
            },
            new Vector3{
                X = 1.3383f,
                Y = 10.96f,
                Z = -0.008521f
            },
            new Vector3{
                X = 1.5867f,
                Y = 10.96f,
                Z = 0.001615f
            },
            new Vector3{
                X = 1.835f,
                Y = 10.96f,
                Z = 0.010657f
            },
            new Vector3{
                X = 2.0833f,
                Y = 10.96f,
                Z = 0.018102f
            },
            new Vector3{
                X = 2.3317f,
                Y = 10.96f,
                Z = 0.023411f
            },
            new Vector3{
                X = 3.0083f,
                Y = 10.96f,
                Z = 0.024777f
            },
            new Vector3{
                X = 3.2567f,
                Y = 10.96f,
                Z = 0.022351f
            },
            new Vector3{
                X = 3.505f,
                Y = 10.96f,
                Z = 0.019196f
            },
            new Vector3{
                X = 3.7533f,
                Y = 10.96f,
                Z = 0.015662f
            },
            new Vector3{
                X = 4.0017f,
                Y = 10.96f,
                Z = 0.012079f
            },
            new Vector3{
                X = 4.6783f,
                Y = 10.96f,
                Z = 0.004218f
            },
            new Vector3{
                X = 4.9267f,
                Y = 10.96f,
                Z = 0.002158f
            },
            new Vector3{
                X = 5.175f,
                Y = 10.96f,
                Z = 0.000479f
            },
            new Vector3{
                X = 5.4233f,
                Y = 10.96f,
                Z = -0.000849f
            },
            new Vector3{
                X = 5.6717f,
                Y = 10.96f,
                Z = -0.001854f
            },
            new Vector3{
                X = 6.3483f,
                Y = 10.96f,
                Z = -0.003233f
            },
            new Vector3{
                X = 6.5967f,
                Y = 10.96f,
                Z = -0.003428f
            },
            new Vector3{
                X = 6.845f,
                Y = 10.96f,
                Z = -0.003514f
            },
            new Vector3{
                X = 7.0933f,
                Y = 10.96f,
                Z = -0.003513f
            },
            new Vector3{
                X = 7.3417f,
                Y = 10.96f,
                Z = -0.003446f
            },
            new Vector3{
                X = 8.0183f,
                Y = 10.96f,
                Z = -0.003081f
            },
            new Vector3{
                X = 8.2667f,
                Y = 10.96f,
                Z = -0.002908f
            },
            new Vector3{
                X = 8.515f,
                Y = 10.96f,
                Z = -0.00272f
            },
            new Vector3{
                X = 8.7633f,
                Y = 10.96f,
                Z = -0.002523f
            },
            new Vector3{
                X = 9.0117f,
                Y = 10.96f,
                Z = -0.002321f
            },
            new Vector3{
                X = 1.3383f,
                Y = 12.33f,
                Z = -0.001717f
            },
            new Vector3{
                X = 1.5867f,
                Y = 12.33f,
                Z = 0.003634f
            },
            new Vector3{
                X = 1.835f,
                Y = 12.33f,
                Z = 0.007585f
            },
            new Vector3{
                X = 2.0833f,
                Y = 12.33f,
                Z = 0.010277f
            },
            new Vector3{
                X = 2.3317f,
                Y = 12.33f,
                Z = 0.011864f
            },
            new Vector3{
                X = 3.0083f,
                Y = 12.33f,
                Z = 0.011933f
            },
            new Vector3{
                X = 3.2567f,
                Y = 12.33f,
                Z = 0.011061f
            },
            new Vector3{
                X = 3.505f,
                Y = 12.33f,
                Z = 0.009876f
            },
            new Vector3{
                X = 3.7533f,
                Y = 12.33f,
                Z = 0.008438f
            },
            new Vector3{
                X = 4.0017f,
                Y = 12.33f,
                Z = 0.006806f
            },
            new Vector3{
                X = 4.6783f,
                Y = 12.33f,
                Z = 0.002253f
            },
            new Vector3{
                X = 4.9267f,
                Y = 12.33f,
                Z = 0.001061f
            },
            new Vector3{
                X = 5.175f,
                Y = 12.33f,
                Z = 0.000153f
            },
            new Vector3{
                X = 5.4233f,
                Y = 12.33f,
                Z = -0.000519f
            },
            new Vector3{
                X = 5.6717f,
                Y = 12.33f,
                Z = -0.001009f
            },
            new Vector3{
                X = 6.3483f,
                Y = 12.33f,
                Z = -0.001731f
            },
            new Vector3{
                X = 6.5967f,
                Y = 12.33f,
                Z = -0.001842f
            },
            new Vector3{
                X = 6.845f,
                Y = 12.33f,
                Z = -0.00189f
            },
            new Vector3{
                X = 7.0933f,
                Y = 12.33f,
                Z = -0.001886f
            },
            new Vector3{
                X = 7.3417f,
                Y = 12.33f,
                Z = -0.001843f
            },
            new Vector3{
                X = 8.0183f,
                Y = 12.33f,
                Z = -0.001624f
            },
            new Vector3{
                X = 8.2667f,
                Y = 12.33f,
                Z = -0.00154f
            },
            new Vector3{
                X = 8.515f,
                Y = 12.33f,
                Z = -0.001452f
            },
            new Vector3{
                X = 8.7633f,
                Y = 12.33f,
                Z = -0.001356f
            },
            new Vector3{
                X = 9.0117f,
                Y = 12.33f,
                Z = -0.001246f
            },
            new Vector3{
                X = 1.3383f,
                Y = 13.7f,
                Z = 0.006018f
            },
            new Vector3{
                X = 1.5867f,
                Y = 13.7f,
                Z = 0.007169f
            },
            new Vector3{
                X = 1.835f,
                Y = 13.7f,
                Z = 0.006431f
            },
            new Vector3{
                X = 2.0833f,
                Y = 13.7f,
                Z = 0.004524f
            },
            new Vector3{
                X = 2.3317f,
                Y = 13.7f,
                Z = 0.002202f
            },
            new Vector3{
                X = 3.0083f,
                Y = 13.7f,
                Z = -3.2e-05f
            },
            new Vector3{
                X = 3.2567f,
                Y = 13.7f,
                Z = 0.000334f
            },
            new Vector3{
                X = 3.505f,
                Y = 13.7f,
                Z = 0.000741f
            },
            new Vector3{
                X = 3.7533f,
                Y = 13.7f,
                Z = 0.000998f
            },
            new Vector3{
                X = 4.0017f,
                Y = 13.7f,
                Z = 0.000937f
            },
            new Vector3{
                X = 4.6783f,
                Y = 13.7f,
                Z = -0.001017f
            },
            new Vector3{
                X = 4.9267f,
                Y = 13.7f,
                Z = -0.001171f
            },
            new Vector3{
                X = 5.175f,
                Y = 13.7f,
                Z = -0.000997f
            },
            new Vector3{
                X = 5.4233f,
                Y = 13.7f,
                Z = -0.000639f
            },
            new Vector3{
                X = 5.6717f,
                Y = 13.7f,
                Z = -0.000249f
            },
            new Vector3{
                X = 6.3483f,
                Y = 13.7f,
                Z = -0.000105f
            },
            new Vector3{
                X = 6.5967f,
                Y = 13.7f,
                Z = -0.000147f
            },
            new Vector3{
                X = 6.845f,
                Y = 13.7f,
                Z = -0.000155f
            },
            new Vector3{
                X = 7.0933f,
                Y = 13.7f,
                Z = -0.000132f
            },
            new Vector3{
                X = 7.3417f,
                Y = 13.7f,
                Z = -8.4e-05f
            },
            new Vector3{
                X = 8.0183f,
                Y = 13.7f,
                Z = 2e-05f
            },
            new Vector3{
                X = 8.2667f,
                Y = 13.7f,
                Z = -1e-05f
            },
            new Vector3{
                X = 8.515f,
                Y = 13.7f,
                Z = -5.1e-05f
            },
            new Vector3{
                X = 8.7633f,
                Y = 13.7f,
                Z = -8.3e-05f
            },
            new Vector3{
                X = 9.0117f,
                Y = 13.7f,
                Z = -8.5e-05f
            },
            new Vector3{
                X = 1.3383f,
                Y = 1.08f,
                Z = -0.00022f
            },
            new Vector3{
                X = 1.5867f,
                Y = 1.08f,
                Z = 0.004275f
            },
            new Vector3{
                X = 1.835f,
                Y = 1.08f,
                Z = 0.007277f
            },
            new Vector3{
                X = 2.0833f,
                Y = 1.08f,
                Z = 0.009049f
            },
            new Vector3{
                X = 2.3317f,
                Y = 1.08f,
                Z = 0.009868f
            },
            new Vector3{
                X = 3.0083f,
                Y = 1.08f,
                Z = 0.009481f
            },
            new Vector3{
                X = 3.2567f,
                Y = 1.08f,
                Z = 0.008852f
            },
            new Vector3{
                X = 3.505f,
                Y = 1.08f,
                Z = 0.00799f
            },
            new Vector3{
                X = 3.7533f,
                Y = 1.08f,
                Z = 0.0069f
            },
            new Vector3{
                X = 4.0017f,
                Y = 1.08f,
                Z = 0.005595f
            },
            new Vector3{
                X = 4.6783f,
                Y = 1.08f,
                Z = 0.001603f
            },
            new Vector3{
                X = 4.9267f,
                Y = 1.08f,
                Z = 0.000618f
            },
            new Vector3{
                X = 5.175f,
                Y = 1.08f,
                Z = -7.8e-05f
            },
            new Vector3{
                X = 5.4233f,
                Y = 1.08f,
                Z = -0.000551f
            },
            new Vector3{
                X = 5.6717f,
                Y = 1.08f,
                Z = -0.000873f
            },
            new Vector3{
                X = 6.3483f,
                Y = 1.08f,
                Z = -0.001415f
            },
            new Vector3{
                X = 6.5967f,
                Y = 1.08f,
                Z = -0.001506f
            },
            new Vector3{
                X = 6.845f,
                Y = 1.08f,
                Z = -0.001543f
            },
            new Vector3{
                X = 7.0933f,
                Y = 1.08f,
                Z = -0.001533f
            },
            new Vector3{
                X = 7.3417f,
                Y = 1.08f,
                Z = -0.001487f
            },
            new Vector3{
                X = 8.0183f,
                Y = 1.08f,
                Z = -0.001288f
            },
            new Vector3{
                X = 8.2667f,
                Y = 1.08f,
                Z = -0.001225f
            },
            new Vector3{
                X = 8.515f,
                Y = 1.08f,
                Z = -0.001162f
            },
            new Vector3{
                X = 8.7633f,
                Y = 1.08f,
                Z = -0.001091f
            },
            new Vector3{
                X = 9.0117f,
                Y = 1.08f,
                Z = -0.001002f
            },
            new Vector3{
                X = 1.3383f,
                Y = 6.028f,
                Z = -0.041526f
            },
            new Vector3{
                X = 1.5867f,
                Y = 6.028f,
                Z = -0.026462f
            },
            new Vector3{
                X = 1.835f,
                Y = 6.028f,
                Z = -0.004406f
            },
            new Vector3{
                X = 2.0833f,
                Y = 6.028f,
                Z = 0.02589f
            },
            new Vector3{
                X = 2.3317f,
                Y = 6.028f,
                Z = 0.057923f
            },
            new Vector3{
                X = 1.3383f,
                Y = 6.302f,
                Z = -0.051994f
            },
            new Vector3{
                X = 1.5867f,
                Y = 6.302f,
                Z = -0.047155f
            },
            new Vector3{
                X = 1.835f,
                Y = 6.302f,
                Z = -0.034688f
            },
            new Vector3{
                X = 2.0833f,
                Y = 6.302f,
                Z = -0.004487f
            },
            new Vector3{
                X = 2.3317f,
                Y = 6.302f,
                Z = 0.039634f
            },
            new Vector3{
                X = 1.3383f,
                Y = 6.576f,
                Z = -0.061154f
            },
            new Vector3{
                X = 1.5867f,
                Y = 6.576f,
                Z = -0.067261f
            },
            new Vector3{
                X = 1.835f,
                Y = 6.576f,
                Z = -0.070612f
            },
            new Vector3{
                X = 2.0833f,
                Y = 6.576f,
                Z = -0.05955f
            },
            new Vector3{
                X = 2.3317f,
                Y = 6.576f,
                Z = 0.010143f
            },
            new Vector3{
                X = 3.0083f,
                Y = 6.028f,
                Z = 0.046855f
            },
            new Vector3{
                X = 3.2567f,
                Y = 6.028f,
                Z = 0.034171f
            },
            new Vector3{
                X = 3.505f,
                Y = 6.028f,
                Z = 0.02471f
            },
            new Vector3{
                X = 3.7533f,
                Y = 6.028f,
                Z = 0.017835f
            },
            new Vector3{
                X = 4.0017f,
                Y = 6.028f,
                Z = 0.012817f
            },
            new Vector3{
                X = 3.0083f,
                Y = 6.302f,
                Z = 0.044956f
            },
            new Vector3{
                X = 3.2567f,
                Y = 6.302f,
                Z = 0.03142f
            },
            new Vector3{
                X = 3.505f,
                Y = 6.302f,
                Z = 0.022264f
            },
            new Vector3{
                X = 3.7533f,
                Y = 6.302f,
                Z = 0.016133f
            },
            new Vector3{
                X = 4.0017f,
                Y = 6.302f,
                Z = 0.011924f
            },
            new Vector3{
                X = 3.0083f,
                Y = 6.576f,
                Z = 0.043102f
            },
            new Vector3{
                X = 3.2567f,
                Y = 6.576f,
                Z = 0.029057f
            },
            new Vector3{
                X = 3.505f,
                Y = 6.576f,
                Z = 0.020323f
            },
            new Vector3{
                X = 3.7533f,
                Y = 6.576f,
                Z = 0.014846f
            },
            new Vector3{
                X = 4.0017f,
                Y = 6.576f,
                Z = 0.011273f
            },
            new Vector3{
                X = 4.6783f,
                Y = 6.028f,
                Z = 0.003646f
            },
            new Vector3{
                X = 4.9267f,
                Y = 6.028f,
                Z = 0.001224f
            },
            new Vector3{
                X = 5.175f,
                Y = 6.028f,
                Z = -0.000747f
            },
            new Vector3{
                X = 5.4233f,
                Y = 6.028f,
                Z = -0.002309f
            },
            new Vector3{
                X = 5.6717f,
                Y = 6.028f,
                Z = -0.003508f
            },
            new Vector3{
                X = 4.6783f,
                Y = 6.302f,
                Z = 0.00355f
            },
            new Vector3{
                X = 4.9267f,
                Y = 6.302f,
                Z = 0.001161f
            },
            new Vector3{
                X = 5.175f,
                Y = 6.302f,
                Z = -0.000794f
            },
            new Vector3{
                X = 5.4233f,
                Y = 6.302f,
                Z = -0.002351f
            },
            new Vector3{
                X = 5.6717f,
                Y = 6.302f,
                Z = -0.00355f
            },
            new Vector3{
                X = 4.6783f,
                Y = 6.576f,
                Z = 0.003491f
            },
            new Vector3{
                X = 4.9267f,
                Y = 6.576f,
                Z = 0.001125f
            },
            new Vector3{
                X = 5.175f,
                Y = 6.576f,
                Z = -0.000821f
            },
            new Vector3{
                X = 5.4233f,
                Y = 6.576f,
                Z = -0.002374f
            },
            new Vector3{
                X = 5.6717f,
                Y = 6.576f,
                Z = -0.003574f
            },
            new Vector3{
                X = 6.3483f,
                Y = 6.028f,
                Z = -0.005336f
            },
            new Vector3{
                X = 6.5967f,
                Y = 6.028f,
                Z = -0.005623f
            },
            new Vector3{
                X = 6.845f,
                Y = 6.028f,
                Z = -0.005756f
            },
            new Vector3{
                X = 7.0933f,
                Y = 6.028f,
                Z = -0.005763f
            },
            new Vector3{
                X = 7.3417f,
                Y = 6.028f,
                Z = -0.005669f
            },
            new Vector3{
                X = 6.3483f,
                Y = 6.302f,
                Z = -0.005385f
            },
            new Vector3{
                X = 6.5967f,
                Y = 6.302f,
                Z = -0.005675f
            },
            new Vector3{
                X = 6.845f,
                Y = 6.302f,
                Z = -0.005809f
            },
            new Vector3{
                X = 7.0933f,
                Y = 6.302f,
                Z = -0.005816f
            },
            new Vector3{
                X = 7.3417f,
                Y = 6.302f,
                Z = -0.005722f
            },
            new Vector3{
                X = 6.3483f,
                Y = 6.576f,
                Z = -0.005414f
            },
            new Vector3{
                X = 6.5967f,
                Y = 6.576f,
                Z = -0.005705f
            },
            new Vector3{
                X = 6.845f,
                Y = 6.576f,
                Z = -0.00584f
            },
            new Vector3{
                X = 7.0933f,
                Y = 6.576f,
                Z = -0.005847f
            },
            new Vector3{
                X = 7.3417f,
                Y = 6.576f,
                Z = -0.0057540000000000004f
            },
            new Vector3{
                X = 8.0183f,
                Y = 6.028f,
                Z = -0.005103f
            },
            new Vector3{
                X = 8.2667f,
                Y = 6.028f,
                Z = -0.004819f
            },
            new Vector3{
                X = 8.515f,
                Y = 6.028f,
                Z = -0.004512f
            },
            new Vector3{
                X = 8.7633f,
                Y = 6.028f,
                Z = -0.004193f
            },
            new Vector3{
                X = 9.0117f,
                Y = 6.028f,
                Z = -0.003875f
            },
            new Vector3{
                X = 8.0183f,
                Y = 6.302f,
                Z = -0.005153f
            },
            new Vector3{
                X = 8.2667f,
                Y = 6.302f,
                Z = -0.004867f
            },
            new Vector3{
                X = 8.515f,
                Y = 6.302f,
                Z = -0.004556f
            },
            new Vector3{
                X = 8.7633f,
                Y = 6.302f,
                Z = -0.004235f
            },
            new Vector3{
                X = 9.0117f,
                Y = 6.302f,
                Z = -0.003914f
            },
            new Vector3{
                X = 8.0183f,
                Y = 6.576f,
                Z = -0.005182f
            },
            new Vector3{
                X = 8.2667f,
                Y = 6.576f,
                Z = -0.004895f
            },
            new Vector3{
                X = 8.515f,
                Y = 6.576f,
                Z = -0.004583f
            },
            new Vector3{
                X = 8.7633f,
                Y = 6.576f,
                Z = -0.00426f
            },
            new Vector3{
                X = 9.0117f,
                Y = 6.576f,
                Z = -0.003937f
            },
            new Vector3{
                X = 1.3383f,
                Y = 7.398f,
                Z = -0.051054f
            },
            new Vector3{
                X = 1.5867f,
                Y = 7.398f,
                Z = -0.045231f
            },
            new Vector3{
                X = 1.835f,
                Y = 7.398f,
                Z = -0.031596f
            },
            new Vector3{
                X = 2.0833f,
                Y = 7.398f,
                Z = -0.000319f
            },
            new Vector3{
                X = 2.3317f,
                Y = 7.398f,
                Z = 0.042145f
            },
            new Vector3{
                X = 1.3383f,
                Y = 7.672f,
                Z = -0.040607f
            },
            new Vector3{
                X = 1.5867f,
                Y = 7.672f,
                Z = -0.024779f
            },
            new Vector3{
                X = 1.835f,
                Y = 7.672f,
                Z = -0.00207f
            },
            new Vector3{
                X = 2.0833f,
                Y = 7.672f,
                Z = 0.028171f
            },
            new Vector3{
                X = 2.3317f,
                Y = 7.672f,
                Z = 0.059232f
            },
            new Vector3{
                X = 1.3383f,
                Y = 7.946f,
                Z = -0.032245f
            },
            new Vector3{
                X = 1.5867f,
                Y = 7.946f,
                Z = -0.010763f
            },
            new Vector3{
                X = 1.835f,
                Y = 7.946f,
                Z = 0.014505f
            },
            new Vector3{
                X = 2.0833f,
                Y = 7.946f,
                Z = 0.040999f
            },
            new Vector3{
                X = 2.3317f,
                Y = 7.946f,
                Z = 0.062102f
            },
            new Vector3{
                X = 3.0083f,
                Y = 7.398f,
                Z = 0.045347f
            },
            new Vector3{
                X = 3.2567f,
                Y = 7.398f,
                Z = 0.031829f
            },
            new Vector3{
                X = 3.505f,
                Y = 7.398f,
                Z = 0.02259f
            },
            new Vector3{
                X = 3.7533f,
                Y = 7.398f,
                Z = 0.016353f
            },
            new Vector3{
                X = 4.0017f,
                Y = 7.398f,
                Z = 0.012045f
            },
            new Vector3{
                X = 3.0083f,
                Y = 7.672f,
                Z = 0.04719f
            },
            new Vector3{
                X = 3.2567f,
                Y = 7.672f,
                Z = 0.034573f
            },
            new Vector3{
                X = 3.505f,
                Y = 7.672f,
                Z = 0.025062f
            },
            new Vector3{
                X = 3.7533f,
                Y = 7.672f,
                Z = 0.018087f
            },
            new Vector3{
                X = 4.0017f,
                Y = 7.672f,
                Z = 0.012964f
            },
            new Vector3{
                X = 3.0083f,
                Y = 7.946f,
                Z = 0.048062f
            },
            new Vector3{
                X = 3.2567f,
                Y = 7.946f,
                Z = 0.036671f
            },
            new Vector3{
                X = 3.505f,
                Y = 7.946f,
                Z = 0.027228f
            },
            new Vector3{
                X = 3.7533f,
                Y = 7.946f,
                Z = 0.019721f
            },
            new Vector3{
                X = 4.0017f,
                Y = 7.946f,
                Z = 0.013881f
            },
            new Vector3{
                X = 4.6783f,
                Y = 7.398f,
                Z = 0.003574f
            },
            new Vector3{
                X = 4.9267f,
                Y = 7.398f,
                Z = 0.001179f
            },
            new Vector3{
                X = 5.175f,
                Y = 7.398f,
                Z = -0.00078f
            },
            new Vector3{
                X = 5.4233f,
                Y = 7.398f,
                Z = -0.002339f
            },
            new Vector3{
                X = 5.6717f,
                Y = 7.398f,
                Z = -0.00354f
            },
            new Vector3{
                X = 4.6783f,
                Y = 7.672f,
                Z = 0.003681f
            },
            new Vector3{
                X = 4.9267f,
                Y = 7.672f,
                Z = 0.001251f
            },
            new Vector3{
                X = 5.175f,
                Y = 7.672f,
                Z = -0.000725f
            },
            new Vector3{
                X = 5.4233f,
                Y = 7.672f,
                Z = -0.002292f
            },
            new Vector3{
                X = 5.6717f,
                Y = 7.672f,
                Z = -0.003494f
            },
            new Vector3{
                X = 4.6783f,
                Y = 7.946f,
                Z = 0.003816f
            },
            new Vector3{
                X = 4.9267f,
                Y = 7.946f,
                Z = 0.001346f
            },
            new Vector3{
                X = 5.175f,
                Y = 7.946f,
                Z = -0.00065f
            },
            new Vector3{
                X = 5.4233f,
                Y = 7.946f,
                Z = -0.002224f
            },
            new Vector3{
                X = 5.6717f,
                Y = 7.946f,
                Z = -0.003429f
            },
            new Vector3{
                X = 6.3483f,
                Y = 7.398f,
                Z = -0.005378f
            },
            new Vector3{
                X = 6.5967f,
                Y = 7.398f,
                Z = -0.005669f
            },
            new Vector3{
                X = 6.845f,
                Y = 7.398f,
                Z = -0.005805f
            },
            new Vector3{
                X = 7.0933f,
                Y = 7.398f,
                Z = -0.005812f
            },
            new Vector3{
                X = 7.3417f,
                Y = 7.398f,
                Z = -0.00572f
            },
            new Vector3{
                X = 6.3483f,
                Y = 7.672f,
                Z = -0.005326f
            },
            new Vector3{
                X = 6.5967f,
                Y = 7.672f,
                Z = -0.005615f
            },
            new Vector3{
                X = 6.845f,
                Y = 7.672f,
                Z = -0.00575f
            },
            new Vector3{
                X = 7.0933f,
                Y = 7.672f,
                Z = -0.005757f
            },
            new Vector3{
                X = 7.3417f,
                Y = 7.672f,
                Z = -0.005665f
            },
            new Vector3{
                X = 6.3483f,
                Y = 7.946f,
                Z = -0.005253f
            },
            new Vector3{
                X = 6.5967f,
                Y = 7.946f,
                Z = -0.00554f
            },
            new Vector3{
                X = 6.845f,
                Y = 7.946f,
                Z = -0.005673f
            },
            new Vector3{
                X = 7.0933f,
                Y = 7.946f,
                Z = -0.005681f
            },
            new Vector3{
                X = 7.3417f,
                Y = 7.946f,
                Z = -0.005589f
            },
            new Vector3{
                X = 8.0183f,
                Y = 7.398f,
                Z = -0.005152f
            },
            new Vector3{
                X = 8.2667f,
                Y = 7.398f,
                Z = -0.004866f
            },
            new Vector3{
                X = 8.515f,
                Y = 7.398f,
                Z = -0.004556f
            },
            new Vector3{
                X = 8.7633f,
                Y = 7.398f,
                Z = -0.004235f
            },
            new Vector3{
                X = 9.0117f,
                Y = 7.398f,
                Z = -0.003914f
            },
            new Vector3{
                X = 8.0183f,
                Y = 7.672f,
                Z = -0.005102f
            },
            new Vector3{
                X = 8.2667f,
                Y = 7.672f,
                Z = -0.004818f
            },
            new Vector3{
                X = 8.515f,
                Y = 7.672f,
                Z = -0.004511f
            },
            new Vector3{
                X = 8.7633f,
                Y = 7.672f,
                Z = -0.004193f
            },
            new Vector3{
                X = 9.0117f,
                Y = 7.672f,
                Z = -0.003875f
            },
            new Vector3{
                X = 8.0183f,
                Y = 7.946f,
                Z = -0.005032f
            },
            new Vector3{
                X = 8.2667f,
                Y = 7.946f,
                Z = -0.004752f
            },
            new Vector3{
                X = 8.515f,
                Y = 7.946f,
                Z = -0.004449f
            },
            new Vector3{
                X = 8.7633f,
                Y = 7.946f,
                Z = -0.004134f
            },
            new Vector3{
                X = 9.0117f,
                Y = 7.946f,
                Z = -0.00382f
            },
            new Vector3{
                X = 0.2275f,
                Y = 0.0f,
                Z = -0.023193f
            },
            new Vector3{
                X = 0.455f,
                Y = 0.0f,
                Z = -0.016102f
            },
            new Vector3{
                X = 0.6825f,
                Y = 0.0f,
                Z = -0.009198f
            },
            new Vector3{
                X = 0.2275f,
                Y = 0.25f,
                Z = -0.025781f
            },
            new Vector3{
                X = 0.455f,
                Y = 0.25f,
                Z = -0.018592f
            },
            new Vector3{
                X = 0.6825f,
                Y = 0.25f,
                Z = -0.011559f
            },
            new Vector3{
                X = 9.6675f,
                Y = 0.0f,
                Z = 0.000156f
            },
            new Vector3{
                X = 9.895f,
                Y = 0.0f,
                Z = 0.000274f
            },
            new Vector3{
                X = 10.1225f,
                Y = 0.0f,
                Z = 0.000396f
            },
            new Vector3{
                X = 9.6675f,
                Y = 0.25f,
                Z = -3.4e-05f
            },
            new Vector3{
                X = 9.895f,
                Y = 0.25f,
                Z = 8.7e-05f
            },
            new Vector3{
                X = 10.1225f,
                Y = 0.25f,
                Z = 0.00021f
            },
            new Vector3{
                X = 0.2275f,
                Y = 0.5f,
                Z = -0.028421f
            },
            new Vector3{
                X = 0.455f,
                Y = 0.5f,
                Z = -0.021099f
            },
            new Vector3{
                X = 0.6825f,
                Y = 0.5f,
                Z = -0.013913f
            },
            new Vector3{
                X = 9.6675f,
                Y = 0.5f,
                Z = -0.000225f
            },
            new Vector3{
                X = 9.895f,
                Y = 0.5f,
                Z = -0.000101f
            },
            new Vector3{
                X = 10.1225f,
                Y = 0.5f,
                Z = 2.6e-05f
            },
            new Vector3{
                X = 0.2275f,
                Y = 0.79f,
                Z = -0.031626f
            },
            new Vector3{
                X = 0.455f,
                Y = 0.79f,
                Z = -0.024099f
            },
            new Vector3{
                X = 0.6825f,
                Y = 0.79f,
                Z = -0.016693f
            },
            new Vector3{
                X = 9.6675f,
                Y = 0.79f,
                Z = -0.000444f
            },
            new Vector3{
                X = 9.895f,
                Y = 0.79f,
                Z = -0.000315f
            },
            new Vector3{
                X = 10.1225f,
                Y = 0.79f,
                Z = -0.000185f
            },
            new Vector3{
                X = 0.2275f,
                Y = 1.37f,
                Z = -0.038636f
            },
            new Vector3{
                X = 0.455f,
                Y = 1.37f,
                Z = -0.030529f
            },
            new Vector3{
                X = 0.6825f,
                Y = 1.37f,
                Z = -0.022516f
            },
            new Vector3{
                X = 0.2275f,
                Y = 2.74f,
                Z = -0.057724f
            },
            new Vector3{
                X = 0.455f,
                Y = 2.74f,
                Z = -0.047688f
            },
            new Vector3{
                X = 0.6825f,
                Y = 2.74f,
                Z = -0.03762f
            },
            new Vector3{
                X = 9.6675f,
                Y = 1.37f,
                Z = -0.000871f
            },
            new Vector3{
                X = 9.895f,
                Y = 1.37f,
                Z = -0.000731f
            },
            new Vector3{
                X = 10.1225f,
                Y = 1.37f,
                Z = -0.000589f
            },
            new Vector3{
                X = 9.6675f,
                Y = 2.74f,
                Z = -0.001792f
            },
            new Vector3{
                X = 9.895f,
                Y = 2.74f,
                Z = -0.001613f
            },
            new Vector3{
                X = 10.1225f,
                Y = 2.74f,
                Z = -0.001434f
            },
            new Vector3{
                X = 0.2275f,
                Y = 4.11f,
                Z = -0.076918f
            },
            new Vector3{
                X = 0.455f,
                Y = 4.11f,
                Z = -0.065441f
            },
            new Vector3{
                X = 0.6825f,
                Y = 4.11f,
                Z = -0.053767f
            },
            new Vector3{
                X = 9.6675f,
                Y = 4.11f,
                Z = -0.002525f
            },
            new Vector3{
                X = 9.895f,
                Y = 4.11f,
                Z = -0.002307f
            },
            new Vector3{
                X = 10.1225f,
                Y = 4.11f,
                Z = -0.00209f
            },
            new Vector3{
                X = 0.2275f,
                Y = 5.48f,
                Z = -0.089696f
            },
            new Vector3{
                X = 0.455f,
                Y = 5.48f,
                Z = -0.078707f
            },
            new Vector3{
                X = 0.6825f,
                Y = 5.48f,
                Z = -0.067627f
            },
            new Vector3{
                X = 9.6675f,
                Y = 5.48f,
                Z = -0.003f
            },
            new Vector3{
                X = 9.895f,
                Y = 5.48f,
                Z = -0.002754f
            },
            new Vector3{
                X = 10.1225f,
                Y = 5.48f,
                Z = -0.002511f
            },
            new Vector3{
                X = 0.2275f,
                Y = 5.754f,
                Z = -0.091037f
            },
            new Vector3{
                X = 0.455f,
                Y = 5.754f,
                Z = -0.080324f
            },
            new Vector3{
                X = 0.6825f,
                Y = 5.754f,
                Z = -0.069619f
            },
            new Vector3{
                X = 9.6675f,
                Y = 5.754f,
                Z = -0.003059f
            },
            new Vector3{
                X = 9.895f,
                Y = 5.754f,
                Z = -0.002809f
            },
            new Vector3{
                X = 10.1225f,
                Y = 5.754f,
                Z = -0.002563f
            },
            new Vector3{
                X = 0.2275f,
                Y = 6.85f,
                Z = -0.092902f
            },
            new Vector3{
                X = 0.455f,
                Y = 6.85f,
                Z = -0.082979f
            },
            new Vector3{
                X = 0.6825f,
                Y = 6.85f,
                Z = -0.07343f
            },
            new Vector3{
                X = 0.2275f,
                Y = 7.124f,
                Z = -0.09279f
            },
            new Vector3{
                X = 0.455f,
                Y = 7.124f,
                Z = -0.082796f
            },
            new Vector3{
                X = 0.6825f,
                Y = 7.124f,
                Z = -0.073138f
            },
            new Vector3{
                X = 9.6675f,
                Y = 6.85f,
                Z = -0.003165f
            },
            new Vector3{
                X = 9.895f,
                Y = 6.85f,
                Z = -0.002909f
            },
            new Vector3{
                X = 10.1225f,
                Y = 6.85f,
                Z = -0.002657f
            },
            new Vector3{
                X = 9.6675f,
                Y = 7.124f,
                Z = -0.003158f
            },
            new Vector3{
                X = 9.895f,
                Y = 7.124f,
                Z = -0.002903f
            },
            new Vector3{
                X = 10.1225f,
                Y = 7.124f,
                Z = -0.002652f
            },
            new Vector3{
                X = 0.2275f,
                Y = 8.22f,
                Z = -0.089514f
            },
            new Vector3{
                X = 0.455f,
                Y = 8.22f,
                Z = -0.078497f
            },
            new Vector3{
                X = 0.6825f,
                Y = 8.22f,
                Z = -0.06738f
            },
            new Vector3{
                X = 0.2275f,
                Y = 9.59f,
                Z = -0.076508f
            },
            new Vector3{
                X = 0.455f,
                Y = 9.59f,
                Z = -0.065055f
            },
            new Vector3{
                X = 0.6825f,
                Y = 9.59f,
                Z = -0.053407f
            },
            new Vector3{
                X = 9.6675f,
                Y = 8.22f,
                Z = -0.003002f
            },
            new Vector3{
                X = 9.895f,
                Y = 8.22f,
                Z = -0.002756f
            },
            new Vector3{
                X = 10.1225f,
                Y = 8.22f,
                Z = -0.002514f
            },
            new Vector3{
                X = 9.6675f,
                Y = 9.59f,
                Z = -0.002529f
            },
            new Vector3{
                X = 9.895f,
                Y = 9.59f,
                Z = -0.002311f
            },
            new Vector3{
                X = 10.1225f,
                Y = 9.59f,
                Z = -0.002096f
            },
            new Vector3{
                X = 0.2275f,
                Y = 10.96f,
                Z = -0.057196f
            },
            new Vector3{
                X = 0.455f,
                Y = 10.96f,
                Z = -0.047239f
            },
            new Vector3{
                X = 0.6825f,
                Y = 10.96f,
                Z = -0.037255f
            },
            new Vector3{
                X = 9.6675f,
                Y = 10.96f,
                Z = -0.001797f
            },
            new Vector3{
                X = 9.895f,
                Y = 10.96f,
                Z = -0.001619f
            },
            new Vector3{
                X = 10.1225f,
                Y = 10.96f,
                Z = -0.001442f
            },
            new Vector3{
                X = 0.2275f,
                Y = 12.33f,
                Z = -0.038268f
            },
            new Vector3{
                X = 0.455f,
                Y = 12.33f,
                Z = -0.030219f
            },
            new Vector3{
                X = 0.6825f,
                Y = 12.33f,
                Z = -0.02226f
            },
            new Vector3{
                X = 9.6675f,
                Y = 12.33f,
                Z = -0.000876f
            },
            new Vector3{
                X = 9.895f,
                Y = 12.33f,
                Z = -0.000736f
            },
            new Vector3{
                X = 10.1225f,
                Y = 12.33f,
                Z = -0.000596f
            },
            new Vector3{
                X = 0.2275f,
                Y = 13.7f,
                Z = -0.023006f
            },
            new Vector3{
                X = 0.455f,
                Y = 13.7f,
                Z = -0.015974f
            },
            new Vector3{
                X = 0.6825f,
                Y = 13.7f,
                Z = -0.009122f
            },
            new Vector3{
                X = 9.6675f,
                Y = 13.7f,
                Z = 0.000155f
            },
            new Vector3{
                X = 9.895f,
                Y = 13.7f,
                Z = 0.000272f
            },
            new Vector3{
                X = 10.1225f,
                Y = 13.7f,
                Z = 0.000393f
            },
            new Vector3{
                X = 0.2275f,
                Y = 1.08f,
                Z = -0.035027f
            },
            new Vector3{
                X = 0.455f,
                Y = 1.08f,
                Z = -0.027238f
            },
            new Vector3{
                X = 0.6825f,
                Y = 1.08f,
                Z = -0.019558f
            },
            new Vector3{
                X = 9.6675f,
                Y = 1.08f,
                Z = -0.00066f
            },
            new Vector3{
                X = 9.895f,
                Y = 1.08f,
                Z = -0.000526f
            },
            new Vector3{
                X = 10.1225f,
                Y = 1.08f,
                Z = -0.00039f
            },
            new Vector3{
                X = 0.2275f,
                Y = 6.028f,
                Z = -0.09195f
            },
            new Vector3{
                X = 0.455f,
                Y = 6.028f,
                Z = -0.081532f
            },
            new Vector3{
                X = 0.6825f,
                Y = 6.028f,
                Z = -0.071244f
            },
            new Vector3{
                X = 0.2275f,
                Y = 6.302f,
                Z = -0.092518f
            },
            new Vector3{
                X = 0.455f,
                Y = 6.302f,
                Z = -0.08236f
            },
            new Vector3{
                X = 0.6825f,
                Y = 6.302f,
                Z = -0.072453f
            },
            new Vector3{
                X = 0.2275f,
                Y = 6.576f,
                Z = -0.092818f
            },
            new Vector3{
                X = 0.455f,
                Y = 6.576f,
                Z = -0.082836f
            },
            new Vector3{
                X = 0.6825f,
                Y = 6.576f,
                Z = -0.073196f
            },
            new Vector3{
                X = 9.6675f,
                Y = 6.028f,
                Z = -0.003105f
            },
            new Vector3{
                X = 9.895f,
                Y = 6.028f,
                Z = -0.002853f
            },
            new Vector3{
                X = 10.1225f,
                Y = 6.028f,
                Z = -0.002604f
            },
            new Vector3{
                X = 9.6675f,
                Y = 6.302f,
                Z = -0.003138f
            },
            new Vector3{
                X = 9.895f,
                Y = 6.302f,
                Z = -0.002884f
            },
            new Vector3{
                X = 10.1225f,
                Y = 6.302f,
                Z = -0.002633f
            },
            new Vector3{
                X = 9.6675f,
                Y = 6.576f,
                Z = -0.003158f
            },
            new Vector3{
                X = 9.895f,
                Y = 6.576f,
                Z = -0.002903f
            },
            new Vector3{
                X = 10.1225f,
                Y = 6.576f,
                Z = -0.002651f
            },
            new Vector3{
                X = 0.2275f,
                Y = 7.398f,
                Z = -0.09246f
            },
            new Vector3{
                X = 0.455f,
                Y = 7.398f,
                Z = -0.082279f
            },
            new Vector3{
                X = 0.6825f,
                Y = 7.398f,
                Z = -0.072339f
            },
            new Vector3{
                X = 0.2275f,
                Y = 7.672f,
                Z = -0.091856f
            },
            new Vector3{
                X = 0.455f,
                Y = 7.672f,
                Z = -0.081408f
            },
            new Vector3{
                X = 0.6825f,
                Y = 7.672f,
                Z = -0.07108f
            },
            new Vector3{
                X = 0.2275f,
                Y = 7.946f,
                Z = -0.090901f
            },
            new Vector3{
                X = 0.455f,
                Y = 7.946f,
                Z = -0.080157f
            },
            new Vector3{
                X = 0.6825f,
                Y = 7.946f,
                Z = -0.069411f
            },
            new Vector3{
                X = 9.6675f,
                Y = 7.398f,
                Z = -0.003139f
            },
            new Vector3{
                X = 9.895f,
                Y = 7.398f,
                Z = -0.002885f
            },
            new Vector3{
                X = 10.1225f,
                Y = 7.398f,
                Z = -0.002634f
            },
            new Vector3{
                X = 9.6675f,
                Y = 7.672f,
                Z = -0.003106f
            },
            new Vector3{
                X = 9.895f,
                Y = 7.672f,
                Z = -0.002854f
            },
            new Vector3{
                X = 10.1225f,
                Y = 7.672f,
                Z = -0.002606f
            },
            new Vector3{
                X = 9.6675f,
                Y = 7.946f,
                Z = -0.00306f
            },
            new Vector3{
                X = 9.895f,
                Y = 7.946f,
                Z = -0.002811f
            },
            new Vector3{
                X = 10.1225f,
                Y = 7.946f,
                Z = -0.002565f
            }
                        },
                        PillarData = [],
                        MaxX = 8.89f,
                        MinX = -1.46f,
                        MaxY = 13.7f,
                        MinY = 0.0f,
                        MaxZ = 0.207596f,
                        CheckPointType = CheckPointEnum.PlateM,
                        MyStrength = 2.08f,
                        СonstLoad = 0.45f,
                        PedestrianLoad = 0.0f,
                        KStrength = 1.0f,
                    },
                    Roadway = new Roadway
                    {
                        LineNumber = 2,
                        RoadHeight = 0.24f,
                        LeftSafeline = 0.15f,
                        RightSafeline = 0.15f,
                        Position_shift = 1.46f,
                    }
                },
                new CCResultMessage()
                {
                    C_isso = 38000331,
                    N = 7,
                    С_nagruzka = 40,
                    Direction = DriveDirection.Bidirection,
                    Snip = SnipEnum.sn62,
                    PassType = PassTypeEnum.Denied,
                    Allowed = 0,
                    Intervals = null,
                    Data = "[{\"x\": 2.36, \"y\": 4.7, \"z\": 0.163, \"load\": 1.63}]"
                }
            }
        };
    }
}
