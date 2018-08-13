using Asphalt.Util;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;

namespace JDElectricJob.Components
{

    // a component that will disable the world object if its not on solid ground
    [Serialized]
    [RequireComponent(typeof(StatusComponent))]
    public class BrokenComponent : WorldObjectComponent
    {
        private StatusElement status;
        private CraftingComponent crafting;

        private float time;
        private float speedTime = 120;

        [Serialized] private bool enabled;
        [Serialized] private float breakChance;
        public override bool Enabled => this.enabled;
        public float BrokenState => this.breakChance;

        public override void Initialize()
        {
            this.status = this.Parent.GetComponent<StatusComponent>().CreateStatusElement();
            this.crafting = this.Parent.GetComponent<CraftingComponent>();
            this.enabled = true;
            this.UpdateStatus();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            this.UpdateEnabled();
        }

        public override void Tick()
        {
            base.Tick();
            if (this.crafting.BottleNecked == false && this.Parent.Operating && crafting.CurrentWorkOrder != null)
            {
                breakChance += JDElectricalPlugin.Conf.Get<float>("BrokenChanceOverTime") * WorldObjectManager.TickDeltaTime * speedTime;
                if (time + (60f / speedTime) < WorldObjectManager.TickStartTime)
                {
                    time = (float)WorldObjectManager.TickStartTime;
                    float reduction = 1f - (JDElectricalPlugin.Conf.Get<float>("CarefulSkillEfficencyPercentPerLevel") / 100f * SkillsUtil.GetSkillLevel(crafting.CurrentWorkOrder.Owner.User, typeof(CarefulWorkerSkill)));
                    if (RandomUtil.Range(0.0f, 1.0f) < breakChance * reduction)
                        Break();
                    UpdateStatus();
                }
            }
        }

        private void Break()
        {
            this.enabled = false;
            this.breakChance = 0;
            UpdateEnabled();
        }

        public void Repair()
        {
            this.enabled = true;
            this.breakChance = 0;
            UpdateEnabled();
        }

        public void InitiliazeValues(float breakChance, bool isBroken)
        {
            this.enabled = !isBroken;
            this.breakChance = breakChance;
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            this.UpdateStatus();
            this.Parent.UpdateEnabledAndOperating();
            this.Parent.SetDirty();
        }

        public static string BreakChanceToString(float breakChance)
        {
            string result = "Break chance ";
            float breakChancePct = breakChance * 100f;
            if (breakChancePct < 0.001f)
                result += "none";
            else if (breakChancePct < 1.0f)
                result += "low";
            else if (breakChancePct < 2.5f)
                result += "medium";
            else
                result += "high";
            return result;

        }

        private void UpdateStatus()
        {
            this.status?.SetStatusMessage(this.enabled, this.enabled ? "Machine work perfectly - " + BreakChanceToString(this.breakChance) : "Machine is broken");
        }
    }

    [Serialized]
    public class BrokenItem<T> : WorldObjectItem<T> where T : WorldObject
    {
        [Serialized] protected float BrokenState;
        [Serialized] protected bool IsBroken;

        public override void OnPickup(WorldObject placedObject)
        {
            BrokenComponent brokenComp = placedObject.GetComponent<BrokenComponent>();
            if (brokenComp != null)
            {
                BrokenState = brokenComp.BrokenState;
                IsBroken = !brokenComp.Enabled;
            }
            base.OnPickup(placedObject);
        }

        public override void OnWorldObjectPlaced(WorldObject placedObject)
        {
            BrokenComponent brokenComp = placedObject.GetComponent<BrokenComponent>();
            if (brokenComp != null)
            {
                brokenComp.InitiliazeValues(BrokenState, IsBroken);
            }
            base.OnWorldObjectPlaced(placedObject);
        }
    }
}