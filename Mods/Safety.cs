using GorillaLocomotion;
using StupidTemplate.Classes;
using StupidTemplate.Notifications;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using static StupidTemplate.Classes.RigManager;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Safety
    {
        public static VRRig reportRig;
        public static void AntiReport(System.Action<VRRig, Vector3> onReport)
        {
            if (!NetworkSystem.Instance.InRoom) return;

            if (reportRig != null)
            {
                onReport?.Invoke(reportRig, reportRig.transform.position);
                reportRig = null;
                return;
            }

            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer) continue;
                Transform report = line.reportButton.gameObject.transform;

                foreach (var vrrig in from vrrig in GorillaParent.instance.vrrigs where !vrrig.isLocal let D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position) let D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position) where D1 < 0.35f || D2 < 0.35f select vrrig)
                    onReport?.Invoke(vrrig, report.transform.position);
            }
        }

        public static float antiReportDelay;
        public static void AntiReportDisconnect()
        {
            AntiReport((vrrig, position) =>
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();

                if (!(Time.time > antiReportDelay)) return;
                antiReportDelay = Time.time + 1f;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, you have been disconnected.");
            });
        }
    }
}
