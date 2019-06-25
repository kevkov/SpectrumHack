/* eslint-disable no-undef */
import React, { useEffect, useState } from 'react';
import { withScriptjs, withGoogleMap, GoogleMap, KmlLayer } from "react-google-maps"
import HeatmapLayer from 'react-google-maps/lib/components/visualization/HeatmapLayer';

const getKmlUri = (showPollution, showSchools, startTime, startLatitude, startLongitude, endLatitude, endLongitude) => {  
  
  var kmlUrl = 'https://spectrummapapi.azurewebsites.net/api/map/1?showPollution=' +
                showPollution +
                '&showSchools=' +
                showSchools +
                '&startTime=' +
                startTime +
                '&startName=NorthGreenwich&startLongitude=0.00447&startLatitude=51.49847&endName=Westminster&endLongitude=-0.13563&endLatitude=51.4975'
                +
                '&rand=' + Math.random();

    // var kmlUrl = 'https://spectrummapapi.azurewebsites.net/api/map/1?' +
    //             'showPollution=' + showPollution +
    //             '&showSchools=' + showSchools +
    //             '&startTime=' + startTime +
    //             '&startName=NorthGreenwich&startLongitude='+startLongitude +
    //             '&startLatitude=' + startLatitude +
    //             '&endName=Westminster&endLongitude=' + endLongitude +
    //             '&endLatitude=' + endLatitude +                
    //             '&rand=' + Math.random();

                console.log('URI:' + kmlUrl);

    return kmlUrl;
}

const RouteMap = withScriptjs(withGoogleMap((props) =>   
{
  const [heatmapData, setheatmapData] = useState(() => []);

  async function FetchPollutionData() {
    const url = 'https://spectrummapapi.azurewebsites.net/api/map/pollution';
  
    return fetch(url)
    .then(response => {
        if (!response.ok) {
            console.log(`*********** error calling endpoint ${url}: statusText: ${response.statusText}`);
            throw new Error(response.statusText)
        }
        return response.json()
    })
    .then(jsonData => {
      console.log('LAST THEN');
      console.log(jsonData.length);
  
      var updatedheatmapData = jsonData.map((item) => {
        return {
          weight: item.weight,
          location: new google.maps.LatLng(item.location.latitude, item.location.longitude)
        }
      });

      setheatmapData(updatedheatmapData);
    });
  }

  useEffect(() => {
    
    if (props.showHeatmap) {      
      FetchPollutionData();
    }
    else {
      setheatmapData([]);
    }
  }, [props.showHeatmap]);

  return <GoogleMap
    defaultZoom={12}
    defaultCenter={{ lat: 51.513329, lng: -0.088950 }}    
  >
    <KmlLayer
      url={getKmlUri(props.showPollution, props.showSchools, props.startTime, props.startLatitude, props.startLongitude, props.endLatitude, props.endLongitude)}
      options={{ preserveViewport: true }}
    />

    <HeatmapLayer
    data={heatmapData}
    options={{dissapting: false, radius: 50}}
     />
  </GoogleMap>
}
))

export default RouteMap;