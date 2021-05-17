using Harmony;
using Klyte.AnxietyReducer.Data;
using Klyte.Commons.Extensors;
using Klyte.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Klyte.AnxietyReducer.Overrides
{

    public class AnxietyReducerOverrides : Redirector, IRedirectable
    {

        private static List<Tuple<Type, string>> methodsToDetour = new List<Tuple<Type, string>>
        {
            Tuple.New(    typeof (CitizenManager.Data)        ,               "Serialize"),
            Tuple.New(    typeof (FiremanAI)          ,       "SetRenderParameters"      ),
            Tuple.New(    typeof (FiremanAI)          ,       "SimulationStep"           ),
            Tuple.New(    typeof (HearseDriverAI)     ,           "SimulationStep"       ),
            Tuple.New(    typeof (HumanAI)            ,       "GetTransportWaitPosition" ),
            Tuple.New(    typeof (HumanAI)            ,       "SimulationStep"           ),
            Tuple.New(    typeof (LivestockAI)        ,           "SimulationStep"       ),
            Tuple.New(    typeof (ParamedicAI)        ,           "SimulationStep"       ),
            Tuple.New(    typeof (ParkWorkerAI)       ,           "SimulationStep"       ),
            Tuple.New(    typeof (PoliceOfficerAI)    ,               "SimulationStep"   ),
            Tuple.New(    typeof (RescueWorkerAI)     ,           "SimulationStep"       ),
            Tuple.New(    typeof (WildlifeAI)         ,       "SimulationStep"           ),
            Tuple.New(typeof(CitizenAI),    "GetPathTargetPosition"           ),
            Tuple.New(typeof(CitizenManager),   "CreateCitizenInstance"       ),
            Tuple.New(typeof(CitizenManager.Data),  "Deserialize"         ),
            Tuple.New(typeof(FiremanAI),    "SetTarget"                       ),
            Tuple.New(typeof(FiremanAI),    "SimulationStep"                  ),
            Tuple.New(typeof(HearseDriverAI),   "SetTarget"                   ),
            Tuple.New(typeof(HearseDriverAI),   "SimulationStep"              ),
            Tuple.New(typeof(HumanAI),  "AddWind"                         ),
            Tuple.New(typeof(HumanAI),  "GetTransportWaitPosition"        ),
            Tuple.New(typeof(HumanAI),  "SimulationStep"                  ),
            Tuple.New(typeof(HumanAI),  "WaitTouristVehicle"              ),
            Tuple.New(typeof(LivestockAI),  "AddWind"                     ),
            Tuple.New(typeof(LivestockAI),  "SimulationStep"              ),
            Tuple.New(typeof(ParamedicAI),  "SetTarget"                   ),
            Tuple.New(typeof(ParamedicAI),  "SimulationStep"              ),
            Tuple.New(typeof(ParkWorkerAI), "SetTarget"                   ),
            Tuple.New(typeof(ParkWorkerAI), "SimulationStep"              ),
            Tuple.New(typeof(PoliceOfficerAI),  "SetTarget"               ),
            Tuple.New(typeof(PoliceOfficerAI),  "SimulationStep"          ),
            Tuple.New(typeof(RescueWorkerAI),   "SetTarget"                   ),
            Tuple.New(typeof(RescueWorkerAI),   "SimulationStep"              ),
            Tuple.New(typeof(ResidentAI),   "SpawnVehicle"                    ),
            Tuple.New(typeof(TouristAI),    "SpawnVehicle"                    ),
            Tuple.New(typeof(WildlifeAI),   "AddWind"                         ),
            Tuple.New(typeof(WildlifeAI),   "SimulationStep"                  ),
        };

        public void Awake()
        {
            MethodInfo trp2 = GetType().GetMethod("DetourCounter", RedirectorUtils.allFlags);
            foreach (Tuple<Type, string> tuple in methodsToDetour)
            {
                foreach (MethodInfo src2 in tuple.First.GetMethods(RedirectorUtils.allFlags).Where(x => x.Name == tuple.Second))
                {
                    LogUtils.DoLog($"TRANSPILE AnxietyReducer Method: {src2} => {trp2}");
                    AddRedirect(src2, null, null, trp2);
                }
            }
            GetHarmonyInstance();
        }

        public static byte GetValueForCounter(ref byte waitCounter)
        {
            if (AnxietyData.Instance.WillDo(SimulationManager.instance.m_currentFrameIndex))
            {
                return (byte)(waitCounter + 1);
            }
            return waitCounter;
        }

        public static byte GetValueForCounterIncreaser(ref byte waitCounter)
        {
            if (AnxietyData.Instance.WillDo(SimulationManager.instance.m_referenceFrameIndex))
            {
                waitCounter++;
            }
            return waitCounter;
        }

        private static FieldInfo waitField = typeof(CitizenInstance).GetField("m_waitCounter");
        private static MethodInfo notIncreaser = typeof(AnxietyReducerOverrides).GetMethod("GetValueForCounter");
        private static MethodInfo increaser = typeof(AnxietyReducerOverrides).GetMethod("GetValueForCounterIncreaser");

        public Redirector RedirectorInstance => this;

        private static IEnumerable<CodeInstruction> DetourCounter(IEnumerable<CodeInstruction> instr)
        {
            var instrList = new List<CodeInstruction>(instr);
            for (int i = 0; i < instrList.Count() - 2; i++)
            {
                if (instrList[i].opcode == OpCodes.Ldfld && instrList[i].operand == waitField && instrList[i + 1].opcode == OpCodes.Ldc_I4_1 && instrList[i + 2].opcode == OpCodes.Add)
                {
                    instrList[i].opcode = OpCodes.Ldflda;
                    instrList[i + 1] = new CodeInstruction(OpCodes.Call, notIncreaser);

                    if (i > 0 && instrList[i - 1].opcode == OpCodes.Dup && i + 4 < instrList.Count() && instrList[i + 3].opcode == OpCodes.Conv_U1 && instrList[i + 4].opcode == OpCodes.Stfld && instrList[i + 4].operand == waitField)
                    {
                        instrList[i + 1].operand = increaser;
                        instrList[i + 2] = new CodeInstruction(OpCodes.Pop);

                        instrList.RemoveAt(i + 4);
                        instrList.RemoveAt(i + 3);
                        instrList.RemoveAt(i - 1);
                    }
                    else
                    {
                        instrList.RemoveAt(i + 2);
                    }
                }
            }
            return instrList;
        }



    }

}
