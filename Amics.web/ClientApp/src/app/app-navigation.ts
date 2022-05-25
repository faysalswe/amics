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
        component: 'Part Master',
        selector: '',
        type: ComponentType.PartMaster
      },
      {
        text: 'Adjust Inventory',
        path: '/adjustInventory',
        title: 'Adjust Inventory',
        component: 'Adjust Inventory',
        selector: '',
        type: ComponentType.ProfileComponent
      },
      {
        text: 'Change Location',
        path: '/changeLocation',
        title: 'Change Location',
        component: 'Change Location',
        selector: '',
        type: ComponentType.TasksComponent
      }
    ]
  }
];
