import { Component, HostBinding, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-svg',
  templateUrl: './svg.component.html',
  styleUrl: './svg.component.css'
})
export class SvgComponent implements OnInit {
  @Input() name!: string;
  @Input() width: number=480;
  @Input() height: number=480;
  @Input() size: number = 480;

  ngOnInit(): void {
    if (!this.width || !this.height) {
      this.width = this.size;
      this.height = this.size;
    }
  }

  @HostBinding('style.width.px')
  get hostWidth(): number {
    return this.width;
  }

  @HostBinding('style.height.px')
  get hostHeight(): number {
    return this.height;
  }
}
