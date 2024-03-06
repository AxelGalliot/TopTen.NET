import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrl: './tabs.component.css'
})
export class TabsComponent implements OnInit {
  @Input() tabs: string[] = []
  @Output() onTabChange = new EventEmitter<number>();
  activatedTab: number = 0;

  constructor() { }

  ngOnInit(): void { }

  setTab(index: number) {
    this.activatedTab = index
    this.onTabChange.emit(this.activatedTab)
  }
}

