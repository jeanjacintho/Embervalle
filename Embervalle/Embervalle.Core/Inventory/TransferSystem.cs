using System;
using Embervalle.Core.Events;

namespace Embervalle.Core.Inventory
{
    /// <summary>Sistema estático para mover itens entre slots de qualquer par de containers, com suporte a empilhamento e troca.</summary>
    public static class TransferSystem
    {
        public static TransferResult Transfer(
            IContainer fromContainer,
            int fromSlot,
            IContainer toContainer,
            int toSlot,
            int quantity = -1)
        {
            ItemSlot source = fromContainer.GetSlot(fromSlot);
            ItemSlot dest = toContainer.GetSlot(toSlot);

            if (source.IsEmpty)
            {
                return TransferResult.Empty;
            }

            if (dest.IsLocked)
            {
                return TransferResult.Blocked;
            }

            int qty = quantity == -1 ? source.Item!.Quantity : quantity;
            qty = Math.Min(qty, source.Item!.Quantity);

            if (dest.IsEmpty)
            {
                ItemInstance? moved = source.Remove(qty);
                if (moved != null)
                {
                    _ = dest.TryAdd(moved);
                }

                NotifyChanged(fromContainer, toContainer);
                return TransferResult.Success;
            }

            if (dest.Item!.ItemId == source.Item!.ItemId && dest.Item.IsStackable)
            {
                ItemInstance? toMove = source.Remove(qty);
                if (toMove == null)
                {
                    return TransferResult.Failed;
                }

                int leftover = dest.TryAdd(toMove);
                if (leftover > 0)
                {
                    toMove.Quantity = leftover;
                    _ = source.TryAdd(toMove);
                }

                NotifyChanged(fromContainer, toContainer);
                return TransferResult.Success;
            }

            if (quantity == -1)
            {
                ItemInstance? sItem = source.Item;
                ItemInstance? dItem = dest.Item;
                source.Item = dItem;
                dest.Item = sItem;
                NotifyChanged(fromContainer, toContainer);
                return TransferResult.Swapped;
            }

            return TransferResult.Failed;
        }

        public static void TransferAll(IContainer from, IContainer to)
        {
            ItemSlot[] slots = from.GetAllSlots();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty)
                {
                    continue;
                }

                ItemInstance stack = slots[i].Item!;
                int leftover = to.TryAdd(stack);
                if (leftover == 0)
                {
                    slots[i].Item = null;
                }
                else
                {
                    stack.Quantity = leftover;
                }
            }

            NotifyChanged(from, to);
        }

        private static void NotifyChanged(IContainer a, IContainer b)
        {
            EventBus.Publish(new ContainerChangedEvent { ContainerId = a.ContainerId });
            EventBus.Publish(new ContainerChangedEvent { ContainerId = b.ContainerId });
        }
    }
}
