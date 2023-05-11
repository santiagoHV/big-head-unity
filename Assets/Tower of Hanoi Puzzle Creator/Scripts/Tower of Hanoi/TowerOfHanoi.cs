using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// This is the main script which manages all 
    /// the tower interactions and animations.
    /// </summary>
    public class TowerOfHanoi : MonoBehaviour
    {
        #region All you really need
        /// <summary>
        /// Event fired when the puzzle has been solved.
        /// </summary>
        public UnityEvent puzzleSolved;

        /// <summary>
        /// Event fired when a disk starts moving.
        /// </summary>
        public UnityEvent diskStartedMoving;

        /// <summary>
        /// Event fired when a disk finishes moving.
        /// </summary>
        public UnityEvent diskEndedMoving;

        /// <summary>
        /// Checks if the puzzle has been solved (all the disks 
        /// are stacked on a different rod than the initial one).
        /// </summary>
        public bool IsSolved()
        {
            return gameLogic.IsSolved();
        }

        /// <summary>
        /// Restarts the game by stacking all disks on the initial rod.
        /// </summary>
        /// <param name="initialRod">The initial rod.</param>
        public void RestartGame(int initialRod = 0)
        {
            diskInMotion = -1;
            gameLogic.Restart(initialRod);
            rodSelection.SetGameState(gameLogic.GetGameStateRepresentation());
            PositionDisks();
        }

        /// <summary>
        /// Gets a string representation of the game state.
        /// You can use this to eventually save and restore the game
        /// or to define your own starting position.
        /// String format: initial rod index + rod index from the biggest to the smallest disk
        /// Example: "01122" (4 disks, starting at rod 0, with the 
        /// following current positions: the 2 biggest ones are at rod 1 and the smallest ones are at the rod 2)
        /// </summary>
        public string GameStateRepresentation
        {
            get
            {
                return gameLogic.GetGameStateRepresentation();
            }
            set
            {
                gameLogic.SetGameState(value);
                rodSelection.SetGameState(value);
                PositionDisks();
            }
        }

        /// <summary>
        /// Selects, deselects, or sets a rod as a target for a disk to move to.
        /// Works only when "Auto Solve" is off (default).
        /// </summary>
        /// <param name="rod">Index of the rod to be clicked.</param>
        public void ClickRod(int rod)
        {
            if (!autoSolve)
                rodSelection.Click(rod);
        }
        #endregion
        #region Basic Fields
        /// <summary>
        /// Rules of the game. Disregards geometry and animation.
        /// </summary>
        GameLogic gameLogic;

        /// <summary>
        /// Disks' transforms, positioned at their centers, from the smallest to the biggest disk.
        /// </summary>
        Transform[] disksTransforms;

        /// <summary>
        /// Rods' transforms, from left to right, positioned at their base.
        /// </summary>
        Transform[] rodsTransforms;  
        
        /// <summary>
        /// Scriptable object, bundled inside the tower prefab, used to store the disks data.
        /// </summary>
        public TowerOfHanoiDisksData disksData;

        /// <summary>
        /// Scriptable object, bundled inside the tower prefab, used to store the base and rods data.
        /// </summary>
        public TowerOfHanoiBaseData baseData;

        /// <summary>
        /// Total number of disks.
        /// </summary>
        public int NumberOfDisks
        {
            get
            {
                return disksTransforms.Length;
            }
        }
        #endregion
        #region Geometry
        /// <summary>
        /// Disks radius from the smallest to the biggest
        /// </summary>
        float[] diskRadius;

        /// <summary>
        /// Height of a disk. All of them have the same height.
        /// </summary>
        float diskHeight;

        /// <summary>
        /// Radius of the smallest disk.
        /// </summary>
        float smallDiskRadius;

        /// <summary>
        /// Radius of the biggest disk.
        /// </summary>
        float bigDiskRadius;

        /// <summary>
        /// Radius of the side of a disk.
        /// </summary>
        float diskSideRadius;

        /// <summary>
        /// Radius difference between two consecutive disks.
        /// </summary>
        float deltaDiskRadius;

        /// <summary>
        /// Height of rod, including its cap (top hemisphere).
        /// </summary>
        float rodHeight;

        /// <summary>
        /// Height of rod, excluding its cap (top hemisphere).
        /// </summary>
        float rodHeightWithoutCap;

        /// <summary>
        /// Rods' radius
        /// </summary>
        float rodRadius;

        /// <summary>
        /// Distance between two consecutive rods.
        /// </summary>
        float distanceBetweenRods;

        /// <summary>
        /// Auxiliary values to improve performance.
        /// </summary>
        float cos30, cos60,
              sin30, sin60,
              tan30, tan60,
              deltaHeight1, deltaHeight2,
              capHeight1, capHeight2;

       /// <summary>
       /// Gets the outline of a disks' stack,
       /// going from the bottom to the top of the rod.
       /// It's used to enable "Magic Particles" 
       /// to fly around the stacks.
       /// </summary>
       /// <param name="stack">Stack index.</param>
       /// <returns></returns>
        AnimationCurve GetStackOutline(int stack)
        {
            AnimationCurve outline = new AnimationCurve();
            Keyframe[] keys;
            int n = 0;

            if (gameLogic.GetNumberOfDisks(stack) > 0)
            {
                bool extraRodKey = ((GetStackHeight(stack) + diskHeight) < rodHeightWithoutCap);
                keys = new Keyframe[2 * gameLogic.GetNumberOfDisks(stack) + 4 + (extraRodKey ? 1 : 0)];

                float y = diskSideRadius;

                foreach (var disk in gameLogic.GetDisks(stack))
                {
                    float diskRadius = GetDiskRadius(disk);
                    keys[n++] = new Keyframe(y, diskRadius);
                    y += deltaHeight1;
                    keys[n++] = new Keyframe(y, diskRadius + (sin30 - 1) * diskSideRadius, -tan60, -tan60);
                    y += deltaHeight2;
                }

                if (extraRodKey)
                    keys[n++] = new Keyframe(y, rodRadius);
            }
            else
            {
                keys = new Keyframe[5];
                keys[n++] = new Keyframe(0, rodRadius);
            }

            keys[n++] = new Keyframe(rodHeightWithoutCap, rodRadius);
            keys[n++] = new Keyframe(capHeight1, rodRadius * sin60, -tan30, -tan30);
            keys[n++] = new Keyframe(capHeight2, rodRadius * sin30, -tan60, -tan60);
            keys[n++] = new Keyframe(rodHeight, 0, -6, -6);
            outline.keys = keys;
            return outline;
        }

        /// <summary>
        /// Gets the radius of a disk.
        /// </summary>
        /// <param name="disk">Disk index.</param>
        /// <returns></returns>
        float GetDiskRadius(int disk)
        {
            return diskRadius[disk];
        }

        /// <summary>
        /// Gets the height of a stack.
        /// </summary>
        /// <param name="stack">Stack index.</param>
        /// <returns></returns>
        float GetStackHeight(int stack)
        {
            return gameLogic.GetNumberOfDisks(stack) * diskHeight;
        }

        /// <summary>
        /// Adjusts the position of the disks' transforms
        /// accordingly to the game logic state.
        /// </summary>
        void PositionDisks()
        {
            for (int rod = 0; rod < 3; ++rod)
            {
                int count = 0;
                Vector3 stackPosition = Vector3.right * (rod - 1) * distanceBetweenRods;
                foreach (int disk in gameLogic.GetDisks(rod))
                {
                    Transform diskTransform = disksTransforms[disk];
                    diskTransform.localPosition = stackPosition +
                                                  Vector3.up * (count + 0.5f) * diskHeight;
                    diskTransform.localRotation = Quaternion.identity;
                    ++count;
                }
            }
        }
        #endregion
        #region Disks' Animation
        /// <summary>
        /// Object used to compute the transform of the disk being animated,
        /// given a certain amout of time after it started moving.
        /// </summary>
        TowerOfHanoiDiskAnimation diskAnimation;

        /// <summary>
        /// When the disk started animating.
        /// </summary>
        float diskAnimationStartTime;

        /// <summary>
        /// Current disk being animated.
        /// A -1 value indicates that no disk is moving.
        /// </summary>
        int diskInMotion = -1;

        /// <summary>
        /// Maximum speed a disk can reach while sliding in or out a rod.
        /// </summary>
        public float onRodDiskSpeed = 1;

        /// <summary>
        /// Acceleration/deceleration a disk goes while sliding in or out a rod.
        /// </summary>
        public float onRodDiskAcceleration = 5f;

        /// <summary>
        /// Variable to control the overal speed of a disk animation.
        /// Can be useful if you use "Auto Solve" and wants the puzzle to
        /// be solved at a lightning speed.
        /// </summary>
        public float framerateMultiplier = 1;

        /// <summary>
        /// Disks' animation clip between the top of two adjacent rods.
        /// It must start at:
        ///     position = (0, 0, 0)
        ///     rotation = identity
        /// and end at:
        ///     position = (positive value, 0, 0)
        ///     rotation = identity
        /// Anything can happen between these points.
        /// When used, the animation is scaled to fit the distance between the rods.
        /// Only the part of the animation clip concerning position and rotation is used.
        /// </summary>
        public AnimationClip nearAnimation;

        /// <summary>
        /// Disks' animation clip between the top of the far left and far right rods.
        /// It must start at:
        ///     position = (0, 0, 0)
        ///     rotation = identity
        /// and end at:
        ///     position = (positive value, 0, 0)
        ///     rotation = identity
        /// Anything can happen between these points.
        /// When used, the animation is scaled to fit the distance between the rods.
        /// Only the part of the animation clip concerning position and rotation is used.
        /// </summary>
        public AnimationClip farAnimation;

        /// <summary>
        /// Checks if some disk is animating.
        /// </summary>
        public bool IsDiskAnimating
        {
            get
            {
                return diskInMotion >= 0;
            }
        }

        /// <summary>
        /// Update the position and rotation of the disk being animated.
        /// </summary>
        void UpdateDiskAnimation()
        {
            float time = Time.time - diskAnimationStartTime;
            var newTransform = diskAnimation.GetTransform(time);
            var diskTransform = disksTransforms[diskInMotion];
            diskTransform.position = rodsTransforms[1].TransformPoint(newTransform.position);
            diskTransform.rotation = rodsTransforms[1].rotation * newTransform.rotation;

            if (time > diskAnimation.Duration)
                diskInMotion = -1;
        }

        /// <summary>
        /// If possible, start a disk animation between two rods.
        /// </summary>
        /// <param name="start">Start rod index.</param>
        /// <param name="end">End rod index.</param>
        public void StartDiskAnimation(int start, int end)
        {
            if (IsDiskAnimating || !gameLogic.CanMove(start, end)) return;
            diskStartedMoving.Invoke();
            diskInMotion = gameLogic.GetTopDisk(start);
            gameLogic.MoveDisk(start, end);

            diskAnimation.StartPosition = Vector2.right * (start - 1) * distanceBetweenRods +
                                          Vector2.up * (gameLogic.GetNumberOfDisks(start) + 0.5f) * diskHeight;
            diskAnimation.EndPosition = Vector2.right * (end - 1) * distanceBetweenRods +
                                        Vector2.up * (gameLogic.GetNumberOfDisks(end) - 0.5f) * diskHeight;
            diskAnimation.UseAnimationBetweenAdjacentRods = (Mathf.Abs(end - start) == 1);
            diskAnimation.OnRodSpeed = onRodDiskSpeed;
            diskAnimation.OnRodAcceleration = onRodDiskAcceleration;
            diskAnimation.FrameRateMultiplier = framerateMultiplier;
            diskAnimationStartTime = Time.time;
        }
        #endregion
        #region Sounds
        /// <summary>
        /// Play sounds when rods are selected, deselected...?
        /// </summary>
        public bool playSounds = true;

        /// <summary>
        /// Audio Source to play the select/deselect rod sounds. 
        /// </summary>
        AudioSource audioSourceRods;

        /// <summary>
        /// Audio Source to play a sound in a loop while a rod is selected.
        /// Can be thought of as the sound of the magic particles.
        /// </summary>
        AudioSource audioSourceMagicParticles;

        /// <summary>
        /// Audio clips for each rod, played when they are selected.
        /// </summary>
        public AudioClip[] selectStartRodAudioClips = new AudioClip[3];

        /// <summary>
        /// Audio clips for each rod, played when they are deselected.
        /// </summary>
        public AudioClip[] deselectStartRodAudioClips = new AudioClip[3];

        /// <summary>
        /// Audio clips for each rod, played when they are set as a disk target rod.
        /// </summary>
        public AudioClip[] selectEndRodAudioClips = new AudioClip[3];

        /// <summary>
        /// Audio clips for each rod, played in loop, while they are selected.
        /// </summary>
        public AudioClip[] magicParticleAudioClips = new AudioClip[3];

        //Control variables for the "FadeSound" coroutine below
        float targetVolume,
              maxVolume,
              soundChangeSpeed;
        public float whileSelectedSoundFadeInSpeed = 1,
                     whileSelectedSoundFadeOutSpeed = 1;
        bool isVolumeChanging = false;

        /// <summary>
        /// Coroutine to fade the in/out the sound played while a rod is selected.
        /// </summary>
        IEnumerator FadeSound()
        {
            isVolumeChanging = true;
            for (;;)
            {
                audioSourceMagicParticles.volume += soundChangeSpeed * Time.deltaTime;
                if (Mathf.Abs(targetVolume - audioSourceMagicParticles.volume) <= 0.01f)
                {
                    audioSourceMagicParticles.volume = targetVolume;
                    if (targetVolume == 0)
                        audioSourceMagicParticles.Stop();
                    isVolumeChanging = false;
                    yield break;
                }
                yield return null;
            }
        }

        /// <summary>
        /// Sets the "FadeSound" coroutine control variables to fade in the sound.
        /// If the coroutine is not already running, starts it.
        /// </summary>
        /// <param name="rod">Selected rod index.</param>
        void FadeInMagicParticleSound(int rod)
        {
            targetVolume = maxVolume;
            soundChangeSpeed = whileSelectedSoundFadeInSpeed;

            audioSourceMagicParticles.clip = magicParticleAudioClips[rod];
            if (!audioSourceMagicParticles.isPlaying)
                audioSourceMagicParticles.Play();

            if (!isVolumeChanging)
                StartCoroutine("FadeSound");
        }

        /// <summary>
        /// Sets the "FadeSound" coroutine control variables to fade out the sound.
        /// If the coroutine is not already running, starts it.
        /// </summary>
        void FadeOutMagicParticleSound()
        {
            targetVolume = 0;
            soundChangeSpeed = -whileSelectedSoundFadeOutSpeed;

            if (!isVolumeChanging)
                StartCoroutine("FadeSound");
        }

        /// <summary>
        /// Play the "rod selected" sounds.
        /// </summary>
        /// <param name="rod">Selected rod index.</param>
        public void PlaySoundSelectStartRod(int rod)
        {
            if (playSounds)
            {
                FadeInMagicParticleSound(rod);
                audioSourceRods.PlayOneShot(selectStartRodAudioClips[rod]);
            }
        }

        /// <summary>
        /// Play the "rod deselected" sounds.
        /// </summary>
        /// <param name="rod">Deselected rod index.</param>
        public void PlaySoundDeselectStartRod(int rod)
        {
            if (playSounds)
            {
                FadeOutMagicParticleSound();
                audioSourceRods.PlayOneShot(deselectStartRodAudioClips[rod]);
            }
        }

        /// <summary>
        /// Plays the "rod selected as a target" sounds.
        /// </summary>
        /// <param name="rod">Selected rod index.</param>
        public void PlaySoundSelectEndRod(int rod)
        {
            if (playSounds)
            {
                FadeOutMagicParticleSound();
                audioSourceRods.PlayOneShot(selectEndRodAudioClips[rod]);
            }
        }

        #endregion
        #region Auto Solve
        /// <summary>
        /// Turn on/off automatic solution of the puzzle.
        /// </summary>
        public bool autoSolve = false;

        /// <summary>
        /// Time delay for the selection of end rod,
        /// after the start rod has been selected.
        /// </summary>
        public float autoSolveEndRodSelectionDelay = 0.9f;

        //Control variables for the "UpdateAutoSolve" routine below
        bool autoSolveSelectEndRod = false;
        float autoSolveSelectEndRodTime;
        GameLogic.Move nextAutoSolveMove;

        /// <summary>
        /// If the game is not solved yet, make the next move!
        /// </summary>
        void UpdateAutoSolve()
        {
            if (!gameLogic.IsSolved())
            {
                if (autoSolveSelectEndRod)
                {
                    if (Time.time > autoSolveSelectEndRodTime)
                    {
                        rodSelection.Click(nextAutoSolveMove.End);
                        autoSolveSelectEndRod = false;
                    }
                }
                else
                {
                    autoSolveSelectEndRod = true;
                    nextAutoSolveMove = gameLogic.NextSolutionMove;
                    rodSelection.Click(nextAutoSolveMove.Start);
                    autoSolveSelectEndRodTime = Time.time + autoSolveEndRodSelectionDelay;
                }
            }
        }
        #endregion
        #region Rod Selection
        /// <summary>
        /// Set this variable to false if you don't want to use the default
        /// "Rod Selection System" (Box Colliders + Click Events created
        /// on initialization). You can also select the rods by using 
        /// the ClickRod(int rod) method."
        /// </summary>
        public bool createRodSelectionSystem = true;

        /// <summary>
        /// Class used to handle quick rod selections.
        /// This way the user doesn't have to wait a disk animation
        /// to complete before he can make the next move.
        /// </summary>
        class RodSelection
        {
            /// <summary>
            /// Auxiliary game logic, so we can check the moves that can
            /// be done in advance and don't interfere with the main game logic.
            /// </summary>
            GameLogic gameLogic = new GameLogic();

            /// <summary>
            /// Moves to be done.
            /// </summary>
            Queue<GameLogic.Move> moves = new Queue<GameLogic.Move>();

            /// <summary>
            /// Current rod selected. A -1 value indicates none is selected.
            /// </summary>
            int startRod = -1;

            /// <summary>
            /// Action to be taken when a rod is selected (play sounds, spawn magic particles...)
            /// </summary>
            public Action<int> StartSelected;

            /// <summary>
            /// Action to be taken when a rod is deselected (play sounds, destroy magic particles...)
            /// </summary>
            public Action<int> StartUnselected;

            /// <summary>
            /// Action to be taken when the destination rod is selected (play sounds, move magic particles...)
            /// </summary>
            public Action<int> EndSelected;

            /// <summary>
            /// Action to be taken when a new move has been selected to be done 
            /// (if no disk animation is happening, the new move starts)
            /// </summary>
            public Action NewMoveEnqueued;

            /// <summary>
            /// Have all the moves been done?
            /// </summary>
            public bool IsMoveQueueEmpty()
            {
                return moves.Count == 0;
            }

            /// <summary>
            /// Gets the next move enqueued.
            /// </summary>
            public GameLogic.Move GetNextMove()
            {
                return moves.Dequeue();
            }

            /// <summary>
            /// Click a rod and perform the appropriate actions.
            /// </summary>
            /// <param name="rod">Rod index.</param>
            public void Click(int rod)
            {
                rod = Math.Max(0, Math.Min(2, rod));
                if (startRod < 0)
                {
                    startRod = rod;
                    StartSelected(rod);
                }
                else
                {
                    if (startRod == rod)
                    {
                        startRod = -1;
                        StartUnselected(rod);
                    }
                    else if (gameLogic.CanMove(startRod, rod))
                    {
                        GameLogic.Move move = new GameLogic.Move(startRod, rod);
                        gameLogic.MoveDisk(move);
                        moves.Enqueue(move);
                        startRod = -1;
                        EndSelected(rod);
                        NewMoveEnqueued();
                    }
                }
            }

            /// <summary>
            /// Updates the game state of the auxiliary game logic (<see cref="gameLogic"/>). 
            /// Used to keep the consistency with the main game logic when required (ex.: game restarts).
            /// </summary>
            /// <param name="state"></param>
            public void SetGameState(string state)
            {
                startRod = -1;
                moves.Clear();
                gameLogic.SetGameState(state);
            }
        }

        /// <summary>
        /// Instance of the <see cref="RodSelection"/> class used 
        /// to control the rod selections.
        /// </summary>
        RodSelection rodSelection;

        /// <summary>
        /// Play the "rod has been selected" effects 
        /// (play sounds, spawn magic particles).
        /// </summary>
        /// <param name="rod">Rod index.</param>
        void PlaySelectStartRodEffects(int rod)
        {
            PlaySoundSelectStartRod(rod);
            SpawnMagicParticles(rod);
        }

        /// <summary>
        /// Play the "rod has been deselected" effects 
        /// (play sounds, destroy magic particles).
        /// </summary>
        /// <param name="rod">Rod index.</param>
        void PlayDeselectStartRodEffects(int rod)
        {
            PlaySoundDeselectStartRod(rod);
            FadeOutAndDestroyMagicParticles();
        }

        /// <summary>
        /// Play the "end rod has been selected" effects 
        /// (play sounds, move magic particles to the rod).
        /// </summary>
        /// <param name="rod">Rod index.</param>
        void PlaySelectEndRodEffects(int rod)
        {
            PlaySoundSelectEndRod(rod);
            SetMagicParticlesTargetRod(rod);
        }
        
        /// <summary>
        /// If no disk is animating and there are enqueued moves, start the next one.
        /// </summary>
        void StartNextMove()
        {
            if (!IsDiskAnimating && !rodSelection.IsMoveQueueEmpty())
            {
                var move = rodSelection.GetNextMove();
                StartDiskAnimation(move.Start, move.End);
            }
        }

        /// <summary>
        /// Click the far left rod.
        /// </summary>
        public void ClickLeftRod()
        {
            ClickRod(0);
        }

        /// <summary>
        /// Click the middle rod.
        /// </summary>
        public void ClickMiddleRod()
        {
            ClickRod(1);
        }

        /// <summary>
        /// Click the far right rod.
        /// </summary>
        public void ClickRightRod()
        {
            ClickRod(2);
        }
        #endregion
        #region Magic Particles
        /// <summary>
        /// Reference to the magic particle prefab.
        /// </summary>
        public MagicParticle magicParticlePrefab;

        /// <summary>
        /// Number of magic particles to be spawn.
        /// </summary>
        [SerializeField]
        int numberOfParticlesToSpawn = 7;

        /// <summary>
        /// Array containing the outlines of each disk stack.
        /// </summary>
        AnimationCurve[] stacksOutlines;

        /// <summary>
        /// Array containing the magic particles of the current rod selection.
        /// </summary>
        MagicParticle[] magicParticles;

        /// <summary>
        /// Updates the array containing the stack outlines (<see cref="stacksOutlines"/>)
        /// </summary>
        void UpdateStacksOutlines()
        {
            for (int n = 0; n < 3; ++n)
                stacksOutlines[n] = GetStackOutline(n);
        }

        /// <summary>
        /// Spawn magic particles around a given rod.
        /// </summary>
        /// <param name="rod">Rod index.</param>
        void SpawnMagicParticles(int rod)
        {
            if (numberOfParticlesToSpawn == 0 || magicParticlePrefab == null) return;
            if (magicParticles.Length != numberOfParticlesToSpawn)
                magicParticles = new MagicParticle[numberOfParticlesToSpawn];

            UpdateStacksOutlines();
            for (int n = 0; n < numberOfParticlesToSpawn; ++n)
            {
                var magicParticle = Instantiate(magicParticlePrefab);
                magicParticle.transform.SetParent(transform, false);

                var particle = magicParticle.GetComponent<MagicParticle>();
                magicParticles[n] = particle;
                particle.StacksOutlines = stacksOutlines;
                particle.RodsTransforms = rodsTransforms;
                particle.CurrentRod = rod;
                particle.TargetRod = rod;
                particle.VerticalPositionRange = new FloatRange(0, rodHeight);
                particle.RandomizePosition();
                particle.RandomizeSpeeds();
                particle.FadeIn();
            }
        }

        /// <summary>
        /// Fade out and destroy the magic particles.
        /// </summary>
        void FadeOutAndDestroyMagicParticles()
        {
            for (int n = 0; n < magicParticles.Length; ++n)
                magicParticles[n].FadeOutAndDestroy();
        }

        /// <summary>
        /// Set the magic particles destination rod.
        /// </summary>
        void SetMagicParticlesTargetRod(int targetRod)
        {
            for (int n = 0; n < magicParticles.Length; ++n)
                magicParticles[n].TargetRod = targetRod;
        }
        #endregion
        #region Initializations
        /// <summary>
        /// Checks if the references to the 2 scriptable objects bundled inside the
        /// tower prefab has not been broken. If it has, alert the developer.
        /// </summary>
        void InitializeData()
        {
            if (disksData == null)
                Debug.LogError("Hanoi tower disks data reference is missing.", transform);
            if (baseData == null)
                Debug.LogError("Hanoi tower base data reference is missing.", transform);
        }

        /// <summary>
        /// Pull out the geometry data from the scriptable objects
        /// and initialze auxiliary geometry variables.
        /// </summary>
        void InitializeGeometry()
        {
            if (disksData != null)
            {
                diskHeight = disksData.DiskHeight;
                smallDiskRadius = disksData.smallDiskRadius;
                bigDiskRadius = disksData.bigDiskRadius;
                diskSideRadius = diskHeight / 2;
                if (disksData.numberOfDisks < 2)
                    deltaDiskRadius = 0;
                else
                    deltaDiskRadius = (bigDiskRadius - smallDiskRadius) / (disksData.numberOfDisks - 1);
            }

            if (baseData != null)
            {
                rodHeight = baseData.rodHeight;
                rodRadius = baseData.rodRadius;
                rodHeightWithoutCap = rodHeight - rodRadius;
                distanceBetweenRods = baseData.distanceBetweenRods;
            }

            float angle30 = Mathf.PI / 6,
                  angle60 = Mathf.PI / 3;

            cos30 = Mathf.Cos(angle30);
            cos60 = Mathf.Tan(angle60);
            sin30 = Mathf.Sin(angle30);
            sin60 = Mathf.Tan(angle60);
            tan30 = sin30 / cos30;
            tan60 = sin60 / cos60;

            deltaHeight1 = diskSideRadius * cos30;
            deltaHeight2 = diskHeight - deltaHeight1;
            capHeight1 = rodHeightWithoutCap + rodRadius * cos60;
            capHeight2 = rodHeightWithoutCap + rodRadius * cos30;
            diskRadius = new float[disksData.numberOfDisks];
            for (int n = 0; n < disksData.numberOfDisks; ++n)
                diskRadius[n] = smallDiskRadius + n * deltaDiskRadius;
        }

        /// <summary>
        /// Creates the rods' game objects and place their 
        /// transform at their bases.
        /// Builds the default "Rod Selection System" if you haven't
        /// unchecked it at the "Events" section in the inspector of
        /// this script.
        /// </summary>
        void InitializeRods()
        {
            rodsTransforms = new Transform[3];
            if (baseData == null)
            {
                for (int n = 0; n < 3; ++n)
                    rodsTransforms[n] = transform;
            }
            else
            {
                for (int n = 0; n < 3; ++n)
                {
                    GameObject rod;
                    if (createRodSelectionSystem)
                    {
                        rod = new GameObject("Rod " + n, typeof(BoxCollider), typeof(TowerOfHanoiClickRouter));
                        var aux = new GameObject("Collider", typeof(BoxCollider), typeof(TowerOfHanoiClickRouter));
                        aux.transform.SetParent(rod.transform);
                    }
                    else
                        rod = new GameObject("Rod " + n);

                    rodsTransforms[n] = rod.transform;
                    rodsTransforms[n].SetParent(transform, false);
                    rodsTransforms[n].transform.localPosition = baseData.GetRodPosition(n);
                }

                if (createRodSelectionSystem)
                {
                    float length0 = distanceBetweenRods / 2 + bigDiskRadius,
                          width0a = 2 * rodRadius,
                          width0b = 2 * bigDiskRadius;

                    Vector3 size0a = new Vector3(length0, rodHeight, width0a);
                    Vector3 center0a = new Vector3(length0 / 2 - bigDiskRadius, rodHeight / 2, 0);
                    Vector3 size0b = new Vector3(length0, 0.001f, width0b);
                    Vector3 center0b = new Vector3(length0 / 2 - bigDiskRadius, -0.0005f, 0);

                    var collider = rodsTransforms[0].GetComponent<BoxCollider>();
                    collider.size = size0a;
                    collider.center = center0a;
                    var e = rodsTransforms[0].GetComponent<TowerOfHanoiClickRouter>().routedClickEvents = new UnityEvent();
                    e.AddListener(ClickLeftRod);
                    collider = rodsTransforms[0].GetChild(0).GetComponent<BoxCollider>();
                    collider.size = size0b;
                    collider.center = center0b;
                    rodsTransforms[0].GetChild(0).GetComponent<TowerOfHanoiClickRouter>().routedClickEvents = e;

                    collider = rodsTransforms[1].GetComponent<BoxCollider>();
                    collider.size = new Vector3(distanceBetweenRods, rodHeight, width0a);
                    collider.center = new Vector3(0, center0a.y, 0);
                    e = rodsTransforms[1].GetComponent<TowerOfHanoiClickRouter>().routedClickEvents = new UnityEvent();
                    e.AddListener(ClickMiddleRod);
                    collider = rodsTransforms[1].GetChild(0).GetComponent<BoxCollider>();
                    collider.size = new Vector3(distanceBetweenRods, 0.001f, width0b);
                    collider.center = new Vector3(0, -0.0005f, 0);
                    rodsTransforms[1].GetChild(0).GetComponent<TowerOfHanoiClickRouter>().routedClickEvents = e;

                    collider = rodsTransforms[2].GetComponent<BoxCollider>();
                    collider.size = size0a;
                    collider.center = new Vector3(-center0a.x, center0a.y, 0);
                    e = rodsTransforms[2].GetComponent<TowerOfHanoiClickRouter>().routedClickEvents = new UnityEvent();
                    e.AddListener(ClickRightRod);
                    collider = rodsTransforms[2].GetChild(0).GetComponent<BoxCollider>();
                    collider.size = size0b;
                    collider.center = new Vector3(-center0b.x, -0.0005f, 0);
                    rodsTransforms[2].GetChild(0).GetComponent<TowerOfHanoiClickRouter>().routedClickEvents = e;
                }
            }
        }

        /// <summary>
        /// Creates a separate game object for each disk
        /// and setup it's mesh and material.
        /// </summary>
        void InitializeDisks()
        {
            if (disksData != null)
            {
                Transform aux = transform.Find("Disks");
                Material[] materials = null;
                disksTransforms = new Transform[disksData.numberOfDisks];

                if (aux == null)
                {
                    aux = new GameObject("Disks").transform;
                    aux.SetParent(transform, false);
                    materials = new Material[disksData.numberOfDisks];
                }
                else
                {
                    var meshRenderer = aux.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        materials = meshRenderer.sharedMaterials;
                        if (materials.Length != NumberOfDisks)
                            Array.Resize(ref materials, NumberOfDisks);
                        Destroy(meshRenderer);
                    }

                    Destroy(aux.GetComponent<MeshFilter>());
                }
                aux.transform.localPosition = baseData.GetRodPosition(1);

                var meshes = disksData.GetMeshes();
                for (int n = 0; n < NumberOfDisks; ++n)
                {
                    var go = new GameObject("Disk" + n, typeof(MeshFilter), typeof(MeshRenderer));
                    go.transform.SetParent(aux, false);
                    go.GetComponent<MeshFilter>().sharedMesh = meshes[n];
                    go.GetComponent<MeshRenderer>().sharedMaterial = materials[n];
                    disksTransforms[n] = go.transform;
                }
            }
            else
            {
                disksTransforms = new Transform[0];
            }
        }

        /// <summary>
        /// Creates and initializes the diskAnimation object.
        /// </summary>
        void InitializeDiskAnimation()
        {
            diskAnimation = new TowerOfHanoiDiskAnimation();
            diskAnimation.OnRodSpeed = onRodDiskSpeed;
            diskAnimation.OnRodAcceleration = onRodDiskAcceleration;
            diskAnimation.FrameRateMultiplier = framerateMultiplier;
            diskAnimation.DiskHeight = diskHeight;
            diskAnimation.RodHeight = rodHeight;
            diskAnimation.AnimationBetweenAdjacentRods = nearAnimation;
            diskAnimation.AnimationBetweenExtremeRods = farAnimation;
        }

        /// <summary>
        /// Creates and initializes the gameLogic object.
        /// </summary>
        void InitializeGameLogic()
        {
            gameLogic = new GameLogic();
            gameLogic.NumberOfDisks = disksTransforms.Length;
        }

        /// <summary>
        /// Initialize the AudioSource references;
        /// Check if the audio clips arrays size are ok;
        /// Initialize some fadeSound coroutine variables.
        /// </summary>
        void InitializeAudio()
        {
            var audioSources = GetComponents<AudioSource>();
            if (audioSources.Length == 0)
            {
                playSounds = false;
            }
            else if (audioSources.Length == 1)
            {
                audioSourceRods = audioSources[0];
                audioSourceMagicParticles = gameObject.AddComponent<AudioSource>();
                audioSourceMagicParticles.playOnAwake = false;
                audioSourceMagicParticles.loop = true;
            }
            else
            {
                audioSourceRods = audioSources[0];
                audioSourceMagicParticles = audioSources[1];
            }

            if (selectStartRodAudioClips.Length != 3)
                selectStartRodAudioClips = new AudioClip[3];
            if (deselectStartRodAudioClips.Length != 3)
                deselectStartRodAudioClips = new AudioClip[3];
            if (selectEndRodAudioClips.Length != 3)
                selectEndRodAudioClips = new AudioClip[3];
            if (magicParticleAudioClips.Length != 3)
                magicParticleAudioClips = new AudioClip[3];

            if (playSounds)
            {
                maxVolume = audioSourceMagicParticles.volume;
            }
        }

        /// <summary>
        /// Checks if a magic particle prefab has been provided, 
        /// otherwise no magic particle will spawn.
        /// Creates the arrays to hold the stacks' oulines
        /// and magic particles.
        /// </summary>
        void InitializeMagicParticles()
        {
            if (magicParticlePrefab == null)
                numberOfParticlesToSpawn = 0;
            
            stacksOutlines = new AnimationCurve[3];
            magicParticles = new MagicParticle[numberOfParticlesToSpawn];
        }

        /// <summary>
        /// Creates and initializes the rodSelection object.
        /// </summary>
        void InitializeRodSelection()
        {
            rodSelection = new RodSelection();
            rodSelection.StartSelected += PlaySelectStartRodEffects;
            rodSelection.StartUnselected += PlayDeselectStartRodEffects;
            rodSelection.EndSelected += PlaySelectEndRodEffects;
            rodSelection.NewMoveEnqueued += StartNextMove;
        }
        #endregion
        #region MonoBehaviour
        /// <summary>
        /// Validate public variable changes made in the inspector.
        /// </summary>
        void OnValidate()
        {
            if (numberOfParticlesToSpawn < 0)
                numberOfParticlesToSpawn = 0;
            if (onRodDiskSpeed < 0)
                onRodDiskSpeed = 0.0001f;
            if (onRodDiskAcceleration < 0)
                onRodDiskAcceleration = 0.0001f;
            if (framerateMultiplier < 0)
                framerateMultiplier = 0;
            if (autoSolveEndRodSelectionDelay < 0)
                autoSolveEndRodSelectionDelay = 0;
            if (whileSelectedSoundFadeInSpeed < 0.0001f)
                whileSelectedSoundFadeInSpeed = 0.0001f;
            if (whileSelectedSoundFadeOutSpeed < 0.0001f)
                whileSelectedSoundFadeOutSpeed = 0.0001f;
        }

        /// <summary>
        /// Initializes everything and resets the game.
        /// </summary>
        void Awake()
        {
            InitializeData();
            InitializeGeometry();
            InitializeRods();
            InitializeDisks();
            InitializeDiskAnimation();
            InitializeGameLogic();
            InitializeAudio();
            InitializeMagicParticles();
            InitializeRodSelection();

            RestartGame();
        }

        /// <summary>
        /// If a disk is animating, updates the animation.
        /// Else, if autosolve is turned on, update autosolve, i.e., 
        /// start the next disk animation when the time has come.
        /// </summary>
        void Update()
        {
            if (IsDiskAnimating)
            {
                UpdateDiskAnimation();

                if (!IsDiskAnimating)
                {
                    diskEndedMoving.Invoke();
                    if (gameLogic.IsSolved())
                        puzzleSolved.Invoke();
                    else
                        StartNextMove();
                }
            }
            else if (autoSolve)
            {
                UpdateAutoSolve();
            }
        }
        #endregion
    }
}