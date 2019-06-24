
export interface LatLng {
    latitude: number
    longitude: number
}

export interface Polyline {
    coordinates: LatLng[]
    strokeWidth: number;
    strokeColor: string;
}

export interface Marker {
    title: string;
    image: string;
    coordinates: LatLng;
}

export interface MapData {
    lines: Polyline[]
    markers: Marker[];
}

export interface Journey {
    id: number
    name: string
    icon: string
    start: LatLng
    startName: string
    end: LatLng
    endName: string
    startTime: string
}

export interface RouteInfo {
    colorInHex: string,
    routeLabel: string,
    pollutionPoint: number,
    pollutionZone: number,
    duration: string,
    travelCost: number,
    schoolCount: number,
    distance: number,
    modeOfTransport: string
}

export interface JourneySettings {
    showPollution: boolean,
    showSchools: boolean,
    startTime: string,
    togglePollution: (showPollution: boolean) => void,
    toggleSchools: (showSchools: boolean) => void,
    setStartTime: (startTime: string) => void
}
