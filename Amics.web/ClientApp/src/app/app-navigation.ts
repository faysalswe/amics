import { ComponentType } from "./pages/models/componentType";

export const navigation = [
  {
    text: 'Home',
    path: '/home',
    icon: 'home',
  },
  {
    text: 'Inventory',
    icon: 'folder',
    items: [
      {
        text: 'Part Master & BOM',
        path: '/profile',
        title: 'Part Master',        
        type: ComponentType.PartMaster
      },
      {
        text: 'Adjust Inventory',
        path: '/adjustInventory',
        title: 'Adjust Inventory',
        type: ComponentType.Profile
      },
      {
        text: 'Change Location',
        path: '/changeLocation',
        title: 'Change Location',
        type: ComponentType.Tasks
      }
    ]
  }
];
