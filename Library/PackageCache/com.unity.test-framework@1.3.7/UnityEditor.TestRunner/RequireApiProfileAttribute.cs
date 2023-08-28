using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEditor.Build;

namespace UnityEditor.TestTools
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    internal class RequireApiProfileAttribute : NUnitAttribute, IApplyToTest
    {
        public ApiCompatibilityLevel[] apiProfiles { get; private set; }

        public RequireApiProfileAttribute(params ApiCompatibilityLevel[] apiProfiles)
        {
            this.apiProfiles = apiProfiles;
        }

        void IApplyToTest.ApplyToTest(Test test)
        {
            test.Properties.Add(PropertyNames.Category, string.Format("ApiProfile({0})", string.Join(", ", apiProfiles.Select(p => p.ToString()).OrderBy(p => p).ToArray())));
<<<<<<<< Updated upstream:Library/PackageCache/com.unity.test-framework@1.3.8/UnityEditor.TestRunner/RequireApiProfileAttribute.cs
#if UNITY_2021_2_OR_NEWER
========
#if UNITY_2021_1_OR_NEWER   
>>>>>>>> Stashed changes:Library/PackageCache/com.unity.test-framework@1.3.7/UnityEditor.TestRunner/RequireApiProfileAttribute.cs
            ApiCompatibilityLevel testProfile = PlayerSettings.GetApiCompatibilityLevel(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.activeBuildTargetGroup));
#else
            ApiCompatibilityLevel testProfile = PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.activeBuildTargetGroup);
#endif

            if (!apiProfiles.Contains(testProfile))
            {
                string skipReason = "Skipping test as it requires a compatible api profile set: " + string.Join(", ", apiProfiles.Select(p => p.ToString()).ToArray());
                test.RunState = RunState.Skipped;
                test.Properties.Add(PropertyNames.SkipReason, skipReason);
            }
        }
    }
}
