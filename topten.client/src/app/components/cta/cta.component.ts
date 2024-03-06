import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-cta',
  templateUrl: './cta.component.html',
  styleUrl: './cta.component.css'
})
export class CTAComponent {
  @Input() label: string = ''
  @Input() isDisabled: boolean = false
  @Output() btnClick = new EventEmitter()

  constructor() { }

  onClick() {
    this.btnClick.emit()
  }
}
