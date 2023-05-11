using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace TowerOfHanoiPuzzle
{
    [CustomEditor(typeof(TowerOfHanoi))]
    public class TowerOfHanoiEditor : Editor
    {
        SerializedProperty puzzleSolved, 
                           diskStartedMoving, 
                           diskEndedMoving;
        SerializedProperty onRodDiskSpeed, 
                           onRodDiskAcceleration, 
                           frameRateMultiplier;
        SerializedProperty nearAnimation, 
                           farAnimation;
        SerializedProperty selectStartRodAudioClips,
                           deselectStartRodAudioClips,
                           selectEndRodAudioClips,
                           magicParticleAudioClips,
                           playSounds,
                           soundFadeInSpeed, 
                           soundFadeOutSpeed;
        SerializedProperty autoSolve, 
                           autoSolveEndRodSelectionDelay,
                           createRodSelectionSystem;
        SerializedProperty magicParticlePrefab,
                           numberOfParticlesToSpawn;

        static GUIContent[] RodsNames = new GUIContent[] { new GUIContent("Rod 0"), new GUIContent("Rod 1"), new GUIContent("Rod 2") };
        static GUIContent onRodDiskAccelerationGUIContent = new GUIContent("Acceleration", "Acceleration of the disk while sliding a rod "),
                          frameRateMultiplierGUIContent = new GUIContent("Framerate Mult.", "Overal animation speed (1 = normal speed)"),
                          onRodDiskSpeedGUIContent = new GUIContent("Speed", "Top speed of the disk while sliding a rod"),
                          diskAnimationBetweenAdjacentRodsGUIContent = new GUIContent("Near Animation", "Disk animation from one rod to its neighbour"),
                          diskAnimationBetweenExtremeRodsGUIContent = new GUIContent("Far Animation", "Disk animation between far left and far right rods"),
                          autoSolveEndRodSelectionDelayGUIContent = new GUIContent("Rod selection delay", "Time delay between start and end rods automatic selection"),
                          magicParticlePrefabGUIContent = new GUIContent("Particle"),
                          numberOfParticlesToSpawnGUIContent = new GUIContent("Spawn Number"),
                          rodSelectionSystemGUIContent = new GUIContent("Default Selection"),
                          soundFadeInSpeedGUIContent = new GUIContent("Fade in speed", "Volume change per second"),
                          soundFadeOutSpeedGUIContent = new GUIContent("Fade out speed", "Volume change per second"),
                          autoSolveGUIContent = new GUIContent("Auto Solve ON");
        static bool foldoutAnimations;
        static bool foldoutMagicParticles;
        static bool foldoutSounds;
        static bool foldoutEvents;
        static bool foldoutAutoSolve;

        public void OnEnable()
        {
            puzzleSolved  = serializedObject.FindProperty("puzzleSolved"); 
            diskStartedMoving  = serializedObject.FindProperty("diskStartedMoving"); 
            diskEndedMoving  = serializedObject.FindProperty("diskEndedMoving"); 
            onRodDiskSpeed  = serializedObject.FindProperty("onRodDiskSpeed"); 
            onRodDiskAcceleration  = serializedObject.FindProperty("onRodDiskAcceleration"); 
            frameRateMultiplier  = serializedObject.FindProperty("framerateMultiplier"); 
            nearAnimation  = serializedObject.FindProperty("nearAnimation"); 
            farAnimation  = serializedObject.FindProperty("farAnimation"); 
            selectStartRodAudioClips  = serializedObject.FindProperty("selectStartRodAudioClips"); 
            deselectStartRodAudioClips  = serializedObject.FindProperty("deselectStartRodAudioClips"); 
            selectEndRodAudioClips  = serializedObject.FindProperty("selectEndRodAudioClips");
            magicParticleAudioClips = serializedObject.FindProperty("magicParticleAudioClips");
            playSounds  = serializedObject.FindProperty("playSounds");
            soundFadeInSpeed = serializedObject.FindProperty("whileSelectedSoundFadeInSpeed");
            soundFadeOutSpeed = serializedObject.FindProperty("whileSelectedSoundFadeOutSpeed");
            autoSolve  = serializedObject.FindProperty("autoSolve"); 
            autoSolveEndRodSelectionDelay  = serializedObject.FindProperty("autoSolveEndRodSelectionDelay");
            createRodSelectionSystem = serializedObject.FindProperty("createRodSelectionSystem");
            magicParticlePrefab  = serializedObject.FindProperty("magicParticlePrefab"); 
            numberOfParticlesToSpawn  = serializedObject.FindProperty("numberOfParticlesToSpawn"); 
        }

        void AudioClipsArrayInspector(SerializedProperty property)
        {
            int size = property.arraySize;
            if (size != 3)
                property.arraySize = 3;
            EditorGUI.indentLevel++;
            for (int i = 0; i < 3; i++)
            {
                var prop = property.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(prop, RodsNames[i]);
            }
            EditorGUI.indentLevel--;
        }
        void ShowAnimationsInspector()
        {
            foldoutAnimations = EditorGUILayout.Foldout(foldoutAnimations, "Disk Animations");
            if (foldoutAnimations)
            {
                EditorGUILayout.PropertyField(frameRateMultiplier, frameRateMultiplierGUIContent);
                EditorGUILayout.PropertyField(onRodDiskAcceleration, onRodDiskAccelerationGUIContent);
                EditorGUILayout.PropertyField(onRodDiskSpeed, onRodDiskSpeedGUIContent);
                EditorGUILayout.Separator();
                EditorGUILayout.PropertyField(nearAnimation, diskAnimationBetweenAdjacentRodsGUIContent);
                EditorGUILayout.PropertyField(farAnimation, diskAnimationBetweenExtremeRodsGUIContent);
            }
        }
        void ShowEventsInspector()
        {
            foldoutEvents = EditorGUILayout.Foldout(foldoutEvents, "Events");
            if (foldoutEvents)
            {
                EditorGUILayout.PropertyField(puzzleSolved);
                EditorGUILayout.PropertyField(diskStartedMoving);
                EditorGUILayout.PropertyField(diskEndedMoving);
                EditorGUILayout.HelpBox("Uncheck the box bellow if you don't want to use the default \"Rod Selection System\" (Box Colliders + Click Events created on initialization). You can also select the rods by script using ClickRod(int rod) method on the TowerOfHanoi script.", MessageType.Info);
                EditorGUILayout.PropertyField(createRodSelectionSystem, rodSelectionSystemGUIContent);
            }
        }
        void ShowAudioInspector()
        {
            foldoutSounds = EditorGUILayout.Foldout(foldoutSounds, "Sounds");
            if (foldoutSounds)
            {
                EditorGUILayout.PropertyField(playSounds);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Select");
                AudioClipsArrayInspector(selectStartRodAudioClips);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Deselect");
                AudioClipsArrayInspector(deselectStartRodAudioClips);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Select End");
                AudioClipsArrayInspector(selectEndRodAudioClips);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("While Selected");
                AudioClipsArrayInspector(magicParticleAudioClips);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("While Selected Fading");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(soundFadeInSpeed, soundFadeInSpeedGUIContent);
                EditorGUILayout.PropertyField(soundFadeOutSpeed, soundFadeOutSpeedGUIContent);
                EditorGUI.indentLevel--;
            }
        }
        void ShowAutoSolveInspector()
        {
            foldoutAutoSolve = EditorGUILayout.Foldout(foldoutAutoSolve, "Auto Solve");
            if (foldoutAutoSolve)
            {
                EditorGUILayout.PropertyField(autoSolve, autoSolveGUIContent);
                EditorGUILayout.PropertyField(autoSolveEndRodSelectionDelay, autoSolveEndRodSelectionDelayGUIContent);
            }
        }
        void ShowMagicParticlesInspector()
        {
            foldoutMagicParticles = EditorGUILayout.Foldout(foldoutMagicParticles, "Magic Particles");
            if (foldoutMagicParticles)
            {
                EditorGUILayout.PropertyField(magicParticlePrefab, magicParticlePrefabGUIContent);
                EditorGUILayout.PropertyField(numberOfParticlesToSpawn, numberOfParticlesToSpawnGUIContent);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ShowEventsInspector();
            ShowMagicParticlesInspector();
            ShowAnimationsInspector();
            ShowAudioInspector();
            ShowAutoSolveInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
}