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
        selector: ''
      },
      {
        text: 'Adjust Inventory',
        path: '/adjustInventory',
        title: 'Adjust Inventory',
        component: 'Adjust Inventory',
        selector: ''
      },
      {
        text: 'Change Location',
        path: '/changeLocation',
        title: 'Change Location',
        component: 'Change Location',
        selector: ''
      }
    ]
  }
];
