using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickmanSelectedState : IStickmanState
{
    private Material outline;
    private Stickman stickman;
    private List<Material> materials;
    private Renderer renderer;
    private Animator animator;
    [Inject(Id = "Stc_Movement")] private StickmanMovementState movementState;
    public StickmanSelectedState(Stickman stickman, Material outline)
    {
        this.stickman = stickman;
        this.outline = outline;
        materials = new List<Material>();
        renderer = stickman.GetComponentInChildren<Renderer>();
        animator = stickman.GetComponent<Animator>();
        renderer.GetSharedMaterials(materials);
    }

    public void OnEnter()
    {
        materials.Add(outline);
        renderer.sharedMaterials = materials.ToArray();
        if (!movementState.IsMoving) animator.SetTrigger("Greetings");
    }

    public void OnExit()
    {
        materials.Remove(outline);
        renderer.sharedMaterials = materials.ToArray();
    }
}
