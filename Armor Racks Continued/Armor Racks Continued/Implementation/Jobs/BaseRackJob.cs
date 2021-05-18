using System.Collections.Generic;
using Verse.AI;

namespace ArmorRacksContinued.Implementation.Jobs
{
    /// <summary>
    /// Provides a base class for the actual rack jobs.
    /// </summary>
    internal class BaseRackJob : JobDriver
    {
        public virtual int WaitTicks { get; }

        /// <summary>
        /// Checks if the job can reserve the armor rack.
        /// </summary>
        /// <param name="errorOnFailed">If the method should error on a fail.</param>
        /// <returns>True if the pawn can reserve the armor rack; false otherwise.</returns>
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            // TargetIndex.A is the armor rack.
            this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            return pawn.Reserve(TargetThingA, job, errorOnFailed: errorOnFailed);
        }

        /// <summary>
        /// Reserves the rack and goes to it.
        /// </summary>
        /// <returns>A bunch of Toils?</returns>
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            var destination = TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch;
            yield return Toils_Goto.GotoThing(TargetIndex.A, destination);
            yield return Toils_General.WaitWith(TargetIndex.A, WaitTicks, true);
        }
    }
}
